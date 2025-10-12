namespace MBA.Educacao.Online.Core.DomainObjects;

/// <summary>
/// Exceção customizada para erros de domínio.
/// </summary>
public class DomainException : Exception
{
    public DomainException()
    { }

    public DomainException(string message) : base(message)
    { }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    { }
}

