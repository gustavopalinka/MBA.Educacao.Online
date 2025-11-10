using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MBA.Educacao.Online.Core.Mediator;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.Core;

public class DomainNotificationHandlerTests
{
    [Fact]
    public async Task Deve_Registrar_Notificacoes()
    {
        var handler = new DomainNotificationHandler();

        await handler.Handle(new DomainNotification("Erro", "Mensagem de erro"), CancellationToken.None);

        handler.HasNotifications().Should().BeTrue();
        handler.GetNotifications().Should().ContainSingle(n => n.Key == "Erro" && n.Value == "Mensagem de erro");
    }

    [Fact]
    public async Task Dispose_Deve_Limpar_Notificacoes()
    {
        var handler = new DomainNotificationHandler();
        await handler.Handle(new DomainNotification("Erro", "Mensagem de erro"), CancellationToken.None);

        handler.Dispose();

        handler.HasNotifications().Should().BeFalse();
        handler.GetNotifications().Should().BeEmpty();
    }
}


