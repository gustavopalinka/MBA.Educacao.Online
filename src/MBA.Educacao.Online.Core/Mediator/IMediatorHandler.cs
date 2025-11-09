using MBA.Educacao.Online.Core.Messages;
using System.Threading.Tasks;

namespace MBA.Educacao.Online.Core.Mediator;

public interface IMediatorHandler
{
    Task<bool> EnviarComando<T>(T command) where T : Command;
    Task PublicarEvento<T>(T evento) where T : Event;
    Task PublicarNotificacao<T>(T notification) where T : DomainNotification;
}

