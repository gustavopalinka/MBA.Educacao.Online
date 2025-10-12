using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.Core.Data;

/// <summary>
/// Interface genérica para repositórios.
/// </summary>
/// <typeparam name="T">Tipo do Aggregate Root</typeparam>
public interface IRepository<T> : IDisposable where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}

