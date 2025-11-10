using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation.Results;
using MBA.Educacao.Online.Core.Mediator;
using Moq;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.Core;

public class CommandHandlerTests
{
    [Trait("Categoria", "Core - CommandHandler")]
    [Fact(DisplayName = "NotificarErros deve publicar notificações")]
    public async Task NotificarErros_Deve_Publicar_Notificacoes()
    {
        var mediator = new Mock<IMediatorHandler>();
        mediator
            .Setup(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()))
            .Returns(Task.CompletedTask);

        var handler = new FakeCommandHandler(mediator.Object);
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Campo", "Erro 1"),
            new ValidationFailure("Campo", "Erro 2")
        });

        await handler.PublicarErros(validationResult);

        mediator.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(2));
    }

    [Trait("Categoria", "Core - CommandHandler")]
    [Fact(DisplayName = "NotificarErro deve publicar notificação única")]
    public async Task NotificarErro_Deve_Publicar_Notificacao()
    {
        var mediator = new Mock<IMediatorHandler>();
        mediator
            .Setup(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()))
            .Returns(Task.CompletedTask);

        var handler = new FakeCommandHandler(mediator.Object);

        await handler.PublicarErro("Key", "Mensagem");

        mediator.Verify(m => m.PublicarNotificacao(It.Is<DomainNotification>(n => n.Key == "Key" && n.Value == "Mensagem")), Times.Once);
    }

    private class FakeCommandHandler : CommandHandler
    {
        public FakeCommandHandler(IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
        }

        public Task PublicarErros(ValidationResult validationResult) => NotificarErros(validationResult);

        public Task PublicarErro(string key, string mensagem) => NotificarErro(key, mensagem);
    }
}


