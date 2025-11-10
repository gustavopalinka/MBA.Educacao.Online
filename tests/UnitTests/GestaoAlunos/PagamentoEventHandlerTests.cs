using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MBA.Educacao.Online.Core.Data;
using MBA.Educacao.Online.GestaoAlunos.Application.EventHandlers;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using MBA.Educacao.Online.Pagamentos.Domain;
using Moq;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.GestaoAlunos;

public class PagamentoEventHandlerTests
{
    [Fact]
    public async Task Deve_Ativar_Matricula_Quando_Pagamento_Confirmado()
    {
        var aluno = CriarAlunoComMatricula(out var matriculaId, out var unitOfWorkMock);

        var alunoRepository = new Mock<IAlunoRepository>();
        alunoRepository.Setup(r => r.ObterAlunoComMatriculas(aluno.Id)).ReturnsAsync(aluno);
        alunoRepository.Setup(r => r.Atualizar(aluno));
        alunoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);

        var handler = new PagamentoEventHandler(alunoRepository.Object);

        var evento = new PagamentoConfirmadoEvent(Guid.NewGuid(), matriculaId, aluno.Id);
        await handler.Handle(evento, CancellationToken.None);

        aluno.Matriculas.Should().ContainSingle(m => m.Id == matriculaId && m.Status == StatusMatricula.Ativa);
        alunoRepository.Verify(r => r.Atualizar(aluno), Times.Once);
        unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public async Task Deve_Cancelar_Matricula_Quando_Pagamento_Rejeitado()
    {
        var aluno = CriarAlunoComMatricula(out var matriculaId, out var unitOfWorkMock);

        var alunoRepository = new Mock<IAlunoRepository>();
        alunoRepository.Setup(r => r.ObterAlunoComMatriculas(aluno.Id)).ReturnsAsync(aluno);
        alunoRepository.Setup(r => r.Atualizar(aluno));
        alunoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);

        var handler = new PagamentoEventHandler(alunoRepository.Object);

        var evento = new PagamentoRejeitadoEvent(Guid.NewGuid(), matriculaId, aluno.Id, "Falha");
        await handler.Handle(evento, CancellationToken.None);

        aluno.Matriculas.Should().ContainSingle(m => m.Id == matriculaId && m.Status == StatusMatricula.Cancelada);
        alunoRepository.Verify(r => r.Atualizar(aluno), Times.Once);
        unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public async Task Deve_Ignorar_Quando_Aluno_Nao_Encontrado()
    {
        var alunoRepository = new Mock<IAlunoRepository>();
        alunoRepository.Setup(r => r.ObterAlunoComMatriculas(It.IsAny<Guid>())).ReturnsAsync((Aluno?)null);

        var handler = new PagamentoEventHandler(alunoRepository.Object);

        var evento = new PagamentoConfirmadoEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        await handler.Handle(evento, CancellationToken.None);

        alunoRepository.Verify(r => r.Atualizar(It.IsAny<Aluno>()), Times.Never);
    }

    private static Aluno CriarAlunoComMatricula(out Guid matriculaId, out Mock<IUnitOfWork> unitOfWorkMock)
    {
        var aluno = new Aluno(Guid.NewGuid(), "Aluno Teste", "aluno@teste.com");
        aluno.MatricularEmCurso(Guid.NewGuid());
        var matricula = aluno.Matriculas.First();
        matriculaId = matricula.Id;

        unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);

        return aluno;
    }
}


