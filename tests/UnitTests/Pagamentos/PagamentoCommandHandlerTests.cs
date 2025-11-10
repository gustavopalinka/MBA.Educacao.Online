using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentAssertions;
using MBA.Educacao.Online.Core.Data;
using MBA.Educacao.Online.Core.Mediator;
using MBA.Educacao.Online.Core.Messages;
using MBA.Educacao.Online.Pagamentos.Application.Commands;
using MBA.Educacao.Online.Pagamentos.Application.Handlers;
using MBA.Educacao.Online.Pagamentos.Domain;
using Moq;
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

        var mediatorHandler = new Mock<IMediatorHandler>();
        var eventosPublicados = new List<Event>();
        mediatorHandler.Setup(m => m.PublicarEvento(It.IsAny<Event>()))
            .Callback<Event>(evt => eventosPublicados.Add(evt))
            .Returns(Task.CompletedTask);
        mediatorHandler.Setup(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()))
            .Returns(Task.CompletedTask);

        var handler = new PagamentoCommandHandler(pagamentoRepository.Object, mediatorHandler.Object);

        var command = new RealizarPagamentoCommand(Guid.NewGuid(), Guid.NewGuid(), 150,
            "5555444433332222", "Aluno Teste", "12/30", "123");

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeTrue();
        pagamentoRepository.Verify(r => r.Adicionar(It.IsAny<Pagamento>()), Times.Once);
        eventosPublicados.Should().ContainSingle();
        eventosPublicados.First().Should().BeOfType<PagamentoConfirmadoEvent>();
    }

    [Fact]
    public async Task Deve_Publicar_Evento_De_Rejeicao_Quando_Pagamento_Nao_Aprovado()
    {
        var pagamentoRepository = new Mock<IPagamentoRepository>();
        pagamentoRepository.Setup(r => r.Adicionar(It.IsAny<Pagamento>()));

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Commit()).ReturnsAsync(true);
        pagamentoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWork.Object);

        var mediatorHandler = new Mock<IMediatorHandler>();
        var eventosPublicados = new List<Event>();
        mediatorHandler.Setup(m => m.PublicarEvento(It.IsAny<Event>()))
            .Callback<Event>(evt => eventosPublicados.Add(evt))
            .Returns(Task.CompletedTask);
        mediatorHandler.Setup(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()))
            .Returns(Task.CompletedTask);

        var handler = new PagamentoCommandHandler(pagamentoRepository.Object, mediatorHandler.Object);

        var command = new RealizarPagamentoCommand(Guid.NewGuid(), Guid.NewGuid(), 150,
            "5555444433332221", "Aluno Teste", "12/30", "123"); // último dígito ímpar -> rejeita

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeTrue();
        pagamentoRepository.Verify(r => r.Adicionar(It.IsAny<Pagamento>()), Times.Once);
        eventosPublicados.Should().ContainSingle();
        eventosPublicados.First().Should().BeOfType<PagamentoRejeitadoEvent>();
    }

    [Fact]
    public async Task Deve_Notificar_Falha_Quando_Commit_Nao_Concluido()
    {
        var pagamentoRepository = new Mock<IPagamentoRepository>();
        pagamentoRepository.Setup(r => r.Adicionar(It.IsAny<Pagamento>()));

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Commit()).ReturnsAsync(false);
        pagamentoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWork.Object);

        var mediatorHandler = new Mock<IMediatorHandler>();
        mediatorHandler.Setup(m => m.PublicarEvento(It.IsAny<Event>()))
            .Returns(Task.CompletedTask);
        mediatorHandler.Setup(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()))
            .Returns(Task.CompletedTask);

        var handler = new PagamentoCommandHandler(pagamentoRepository.Object, mediatorHandler.Object);

        var command = new RealizarPagamentoCommand(Guid.NewGuid(), Guid.NewGuid(), 150,
            "5555444433332222", "Aluno Teste", "12/30", "123");

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeFalse();
        mediatorHandler.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
        mediatorHandler.Verify(m => m.PublicarEvento(It.IsAny<Event>()), Times.Never);
    }

    [Fact]
    public async Task Deve_Confirmar_Pagamento_Existente()
    {
        var pagamento = new Pagamento(Guid.NewGuid(), Guid.NewGuid(), 200m,
            new DadosCartao("5555444433332222", "Aluno Teste", "12/30", "123"));

        var pagamentoRepository = new Mock<IPagamentoRepository>();
        pagamentoRepository.Setup(r => r.ObterPorId(pagamento.Id)).ReturnsAsync(pagamento);
        pagamentoRepository.Setup(r => r.Atualizar(It.IsAny<Pagamento>()));

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Commit()).ReturnsAsync(true);
        pagamentoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWork.Object);

        var mediatorHandler = new Mock<IMediatorHandler>();
        var eventosPublicados = new List<Event>();
        mediatorHandler.Setup(m => m.PublicarEvento(It.IsAny<Event>()))
            .Callback<Event>(evt => eventosPublicados.Add(evt))
            .Returns(Task.CompletedTask);
        mediatorHandler.Setup(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()))
            .Returns(Task.CompletedTask);

        var handler = new PagamentoCommandHandler(pagamentoRepository.Object, mediatorHandler.Object);
        var command = new ConfirmarPagamentoCommand(pagamento.Id);

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeTrue();
        pagamento.EstaConfirmado().Should().BeTrue();
        pagamentoRepository.Verify(r => r.Atualizar(pagamento), Times.Once);
        unitOfWork.Verify(u => u.Commit(), Times.Once);
        eventosPublicados.Should().ContainSingle().Which.Should().BeOfType<PagamentoConfirmadoEvent>();
    }

    [Fact]
    public async Task Nao_Deve_Confirmar_Quando_Pagamento_Inexistente()
    {
        var pagamentoRepository = new Mock<IPagamentoRepository>();
        pagamentoRepository.Setup(r => r.ObterPorId(It.IsAny<Guid>())).ReturnsAsync((Pagamento?)null);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Commit()).ReturnsAsync(true);
        pagamentoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWork.Object);

        var mediatorHandler = new Mock<IMediatorHandler>();
        mediatorHandler.Setup(m => m.PublicarEvento(It.IsAny<Event>()))
            .Returns(Task.CompletedTask);
        mediatorHandler.Setup(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()))
            .Returns(Task.CompletedTask);

        var handler = new PagamentoCommandHandler(pagamentoRepository.Object, mediatorHandler.Object);
        var command = new ConfirmarPagamentoCommand(Guid.NewGuid());

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeFalse();
        pagamentoRepository.Verify(r => r.Atualizar(It.IsAny<Pagamento>()), Times.Never);
        mediatorHandler.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
    }
}

