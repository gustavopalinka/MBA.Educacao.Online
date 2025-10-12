using MediatR;

namespace MBA.Educacao.Online.Core.Messages;

/// <summary>
/// Classe base para todos os Domain Events.
/// </summary>
public abstract class Event : Message, INotification
{
    public DateTime Timestamp { get; private set; }

    protected Event()
    {
        Timestamp = DateTime.Now;
    }
}

