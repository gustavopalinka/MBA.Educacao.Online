namespace MBA.Educacao.Online.Core.Messages;

/// <summary>
/// Classe base para todas as mensagens (Commands e Events).
/// </summary>
public abstract class Message
{
    public string MessageType { get; protected set; }
    public Guid AggregateId { get; protected set; }

    protected Message()
    {
        MessageType = GetType().Name;
    }
}

