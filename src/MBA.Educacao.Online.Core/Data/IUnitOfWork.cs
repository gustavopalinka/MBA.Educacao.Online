namespace MBA.Educacao.Online.Core.Data;

/// <summary>
/// Unidade de trabalho para controle transacional.
/// </summary>
public interface IUnitOfWork
{
    Task<bool> Commit();
}

