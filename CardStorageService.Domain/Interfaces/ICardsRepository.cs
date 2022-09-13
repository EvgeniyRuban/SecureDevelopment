﻿namespace CardStorageService.Domain;

public interface ICardsRepository
{
    Task<Guid> Add(CardToCreate cardToCreate, CancellationToken cancellationToken);
    Task<CardResponse> Get(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<CardResponse>> GetAll(CancellationToken cancellationToken);
    Task Update(CardToUpdate cardToUpdate, CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
}