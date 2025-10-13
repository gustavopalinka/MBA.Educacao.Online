using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.Core.Data;

public interface IRepository<T> : IDisposable where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}