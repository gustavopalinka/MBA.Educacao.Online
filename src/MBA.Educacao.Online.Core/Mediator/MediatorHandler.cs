using MBA.Educacao.Online.Core.Messages;
using MediatR;
using System.Threading.Tasks;

namespace MBA.Educacao.Online.Core.Mediator;

public class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;

    public MediatorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> EnviarComando<T>(T command) where T : Command
    {
        return await _mediator.Send(command);
    }

    public async Task PublicarEvento<T>(T evento) where T : Event
    {
        await _mediator.Publish(evento);
    }

    public async Task PublicarNotificacao<T>(T notification) where T : DomainNotification
    {
        await _mediator.Publish(notification);
    }
}

