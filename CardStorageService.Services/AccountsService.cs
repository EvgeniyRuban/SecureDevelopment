using Microsoft.Extensions.Logging;
using AutoMapper;
using CardStorageService.Domain;

namespace CardStorageService.Services;

public sealed class AccountsService : IAccountsService
{
    private readonly IAccountsRepository _accountsRepository;
    private readonly ILogger<AccountsService> _logger;
    private readonly IMapper _mapper;

    public AccountsService(
        IAccountsRepository accountsRepository, 
        ILogger<AccountsService> logger,
        IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(accountsRepository, nameof(accountsRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

        _accountsRepository = accountsRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Guid> Add(AccountToCreate accountToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(accountToCreate, nameof(accountToCreate));

        var result = PasswordUtils.CreatePasswordHash(accountToCreate.Password);

        return await _accountsRepository.Add(_mapper.Map<Account>(result), cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken) 
        => await _accountsRepository.Delete(id, cancellationToken);

    public async Task<AccountResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var account = await _accountsRepository.Get(id, cancellationToken);

        ArgumentNullException.ThrowIfNull(account, nameof(account));

        return _mapper.Map<AccountResponse>(account);
    }

    public async Task<IEnumerable<AccountResponse>> GetAll(CancellationToken cancellationToken)
    {
        var accounts = await _accountsRepository.GetAll(cancellationToken);

        ArgumentNullException.ThrowIfNull(accounts, nameof(accounts));

        return _mapper.Map<List<AccountResponse>>(accounts);
    }

    public async Task Update(AccountToUpdate accountToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(accountToUpdate, nameof(accountToUpdate));

        await _accountsRepository.Update(_mapper.Map<Account>(accountToUpdate), cancellationToken);
    }
}