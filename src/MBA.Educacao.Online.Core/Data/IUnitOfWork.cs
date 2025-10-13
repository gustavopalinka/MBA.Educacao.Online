namespace MBA.Educacao.Online.Core.Data;

public interface IUnitOfWork
{
    Task<bool> Commit();
}