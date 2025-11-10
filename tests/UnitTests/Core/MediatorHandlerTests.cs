using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MBA.Educacao.Online.Core.Mediator;
using MBA.Educacao.Online.Core.Messages;
using MediatR;
using Moq;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.Core;

public class MediatorHandlerTests
{
    [Trait("Categoria", "Core - Mediator")]
    [Fact(DisplayName = "EnviarComando deve chamar mediator.Send")]
    public async Task EnviarComando_Deve_Chamar_Mediator_Send()
    {
        var mediator = new Mock<IMediator>();
        mediator
            .Setup(m => m.Send(It.IsAny<IRequest<bool>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new MediatorHandler(mediator.Object);
        var command = new FakeCommand();

        var resultado = await handler.EnviarComando(command);

        resultado.Should().BeTrue();
        mediator.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Trait("Categoria", "Core - Mediator")]
    [Fact(DisplayName = "PublicarEvento deve chamar mediator.Publish")]
    public async Task PublicarEvento_Deve_Chamar_Mediator_Publish()
    {
        var mediator = new Mock<IMediator>();
        mediator
            .Setup(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new MediatorHandler(mediator.Object);
        var evento = new FakeEvent();

        await handler.PublicarEvento(evento);

        mediator.Verify(m => m.Publish(evento, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Trait("Categoria", "Core - Mediator")]
    [Fact(DisplayName = "PublicarNotificacao deve chamar mediator.Publish")]
    public async Task PublicarNotificacao_Deve_Chamar_Mediator_Publish()
    {
        var mediator = new Mock<IMediator>();
        mediator
            .Setup(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new MediatorHandler(mediator.Object);
        var notification = new DomainNotification("Erro", "Mensagem");

        await handler.PublicarNotificacao(notification);

        mediator.Verify(m => m.Publish(notification, It.IsAny<CancellationToken>()), Times.Once);
    }

    private class FakeCommand : Command
    {
        public override bool EhValido() => true;
    }

    private class FakeEvent : Event
    {
    }
}


