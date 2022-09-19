using Microsoft.Extensions.Logging;
using AutoMapper;
using CardStorageService.Domain;

namespace CardStorageService.Services;

public sealed class AccountsSessionsService : IAccountsSessionsService
{
    private readonly IAccountsSessionsRepository _accountsSessionsRepository;
    private readonly ILogger<AccountsSessionsService> _logger;
    private readonly IMapper _mapper;

    public AccountsSessionsService(
        IAccountsSessionsRepository accountsSessionRepository, 
        ILogger<AccountsSessionsService> logger,
        IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(accountsSessionRepository, nameof(accountsSessionRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

        _accountsSessionsRepository = accountsSessionRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Guid> Add(AccountSessionToCreate accountSessionToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(accountSessionToCreate, nameof(accountSessionToCreate));

        return await _accountsSessionsRepository.Add(
                                                    _mapper.Map<AccountSession>(accountSessionToCreate), 
                                                    cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken) 
        => await _accountsSessionsRepository.Delete(id, cancellationToken);

    public async Task<AccountSessionResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var accountSession = await _accountsSessionsRepository.Get(id, cancellationToken); 

        ArgumentNullException.ThrowIfNull(accountSession, nameof(accountSession));

        return _mapper.Map<AccountSessionResponse>(accountSession);
    }

    public async Task<IEnumerable<AccountSessionResponse>> GetAll(CancellationToken cancellationToken)
    {
        var accountsSessions = await _accountsSessionsRepository.GetAll(cancellationToken);

        ArgumentNullException.ThrowIfNull(accountsSessions, nameof(accountsSessions));

        return _mapper.Map<List<AccountSessionResponse>>(accountsSessions);
    }
}