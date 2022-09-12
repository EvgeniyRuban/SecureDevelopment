using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CardStorageService.Domain;

namespace CardStorageService.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class CardsController : ControllerBase
{
    private readonly ICardsRepository _repository;
    private readonly ILogger<CardsController> _logger;

    public CardsController(ICardsRepository repository, ILogger<CardsController> logger)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _repository = repository;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CardResponse>> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        CardResponse card = null;

        try
        {
            card = await _repository.Get(id, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return card is null ? NotFound() : Ok(card);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CardResponse>>> GetAll(CancellationToken cancellationToken)
    {
        IEnumerable<CardResponse> cards = null;

        try
        {
            cards = await _repository.GetAll(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return cards is null ? NotFound() : Ok(cards);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Add([FromQuery] CardToCreate cardToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cardToCreate, nameof(cardToCreate));

        Guid? newCardId = null;
        try
        {
            newCardId = await _repository.Add(cardToCreate, cancellationToken);
        }
        catch(OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return newCardId is null ? NotFound() : Ok(newCardId);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromQuery]CardToUpdate cardToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cardToUpdate, nameof(cardToUpdate));

        try
        {
            await _repository.Update(cardToUpdate, cancellationToken);
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
    public async Task<ActionResult> Delete([FromRoute] Guid cardId, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.Delete(cardId, cancellationToken);
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