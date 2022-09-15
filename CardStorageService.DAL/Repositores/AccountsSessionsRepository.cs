using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CardStorageService.Domain;

namespace CardStorageService.DAL;

public sealed class AccountsSessionsRepository : IAccountsSessionsRepository
{
    private readonly CardsStorageServiceDbContext _dbContext;
    private readonly ILogger<AccountsSessionsRepository> _logger;

    public AccountsSessionsRepository(
        CardsStorageServiceDbContext dbContext, 
        ILogger<AccountsSessionsRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));    
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));  

        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Guid> Add(AccountSession accountSessionToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(accountSessionToCreate, nameof(accountSessionToCreate));
        cancellationToken.ThrowIfCancellationRequested();

        var accountSession = await _dbContext.AccountsSessions.AddAsync(accountSessionToCreate, cancellationToken);

        ArgumentNullException.ThrowIfNull(accountSession, nameof(accountSession));
        cancellationToken.ThrowIfCancellationRequested();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return accountSession.Entity.Id;
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var accountSession = await _dbContext.AccountsSessions.FirstOrDefaultAsync(a =>
                                                        a.Id == id,
                                                        cancellationToken);

        ArgumentNullException.ThrowIfNull(accountSession, nameof(accountSession));
        _dbContext.Remove(accountSession);

        cancellationToken.ThrowIfCancellationRequested();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<AccountSession> Get(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var accountSession = await _dbContext.AccountsSessions.FirstOrDefaultAsync(a =>
                                                        a.Id == id,
                                                        cancellationToken);

        ArgumentNullException.ThrowIfNull(accountSession, nameof(accountSession));
        cancellationToken.ThrowIfCancellationRequested();

        return accountSession;
    }

    public async Task<AccountSession> Get(string sessionToken, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var accountSession = await _dbContext.AccountsSessions.FirstOrDefaultAsync(a =>
                                                        a.SessionToken == sessionToken,
                                                        cancellationToken);

        ArgumentNullException.ThrowIfNull(accountSession, nameof(accountSession));
        cancellationToken.ThrowIfCancellationRequested();

        return accountSession;
    }

    public async Task<IEnumerable<AccountSession>> GetAll(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var accountsSessions = await _dbContext.AccountsSessions.ToListAsync(cancellationToken);
        
        ArgumentNullException.ThrowIfNull(accountsSessions, nameof(accountsSessions));
        cancellationToken.ThrowIfCancellationRequested();

        return accountsSessions;
    }
}