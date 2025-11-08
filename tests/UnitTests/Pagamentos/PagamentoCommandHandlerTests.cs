using FluentAssertions;
using MBA.Educacao.Online.Core.Messages;
using Moq;
using MBA.Educacao.Online.Core.Data;
using MBA.Educacao.Online.Pagamentos.Application.Commands;
using MBA.Educacao.Online.Pagamentos.Application.Handlers;
using MBA.Educacao.Online.Pagamentos.Domain;
using MediatR;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.Pagamentos;

public class PagamentoCommandHandlerTests
{
    [Fact]
    public async Task Deve_Publicar_Evento_Quando_Pagamento_Aprovado()
    {
        var pagamentoRepository = new Mock<IPagamentoRepository>();
        pagamentoRepository.Setup(r => r.Adicionar(It.IsAny<Pagamento>()));

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Commit()).ReturnsAsync(true);
        pagamentoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWork.Object);

        var mediator = new Mock<IMediator>();
        var eventosPublicados = new List<object>();
        mediator.Setup(m => m.Publish(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Callback<Event, CancellationToken>((evt, _) => eventosPublicados.Add(evt))
                .Returns(Task.CompletedTask);

        mediator.Setup(m => m.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .Callback<object, CancellationToken>((evt, _) => eventosPublicados.Add(evt))
                .Returns(Task.CompletedTask);

        var handler = new PagamentoCommandHandler(pagamentoRepository.Object, mediator.Object);

        var command = new RealizarPagamentoCommand(Guid.NewGuid(), Guid.NewGuid(), 150,
            "5555444433332222", "Aluno Teste", "12/30", "123");

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeTrue();
        pagamentoRepository.Verify(r => r.Adicionar(It.IsAny<Pagamento>()), Times.Once);
        eventosPublicados.Should().ContainSingle();
        eventosPublicados.First().Should().BeOfType<PagamentoConfirmadoEvent>();
    }
}

