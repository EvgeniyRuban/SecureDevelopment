using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CardStorageService.Domain;
using Microsoft.AspNetCore.Authorization;

namespace CardStorageService.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class CardsController : ControllerBase
{
    private readonly ICardsService _cardsService;
    private readonly ILogger<CardsController> _logger;

    public CardsController(ICardsService cardsService, ILogger<CardsController> logger)
    {
        ArgumentNullException.ThrowIfNull(cardsService, nameof(cardsService));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _cardsService = cardsService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CardResponse>> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        CardResponse card = null;

        try
        {
            card = await _cardsService.Get(id, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok(card);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CardResponse>>> GetAll(CancellationToken cancellationToken)
    {
        IEnumerable<CardResponse> cards = null;

        try
        {
            cards = await _cardsService.GetAll(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok(cards);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Add([FromBody] CardToCreate cardToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cardToCreate, nameof(cardToCreate));

        Guid? newCardId = null;
        try
        {
            newCardId = await _cardsService.Add(cardToCreate, cancellationToken);
        }
        catch(OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok(newCardId);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] CardToUpdate cardToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cardToUpdate, nameof(cardToUpdate));

        try
        {
            await _cardsService.Update(cardToUpdate, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _cardsService.Delete(id, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok();
    }
}