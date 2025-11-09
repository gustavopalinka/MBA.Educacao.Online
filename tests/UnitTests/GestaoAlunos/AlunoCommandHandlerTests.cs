using System;
using System.Threading;
using FluentAssertions;
using MBA.Educacao.Online.Core.Data;
using MBA.Educacao.Online.Core.Mediator;
using MBA.Educacao.Online.GestaoAlunos.Application.Commands;
using MBA.Educacao.Online.GestaoAlunos.Application.Handlers;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using Moq;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.GestaoAlunos;

public class AlunoCommandHandlerTests
{
    [Fact]
    public async Task Deve_Matricular_Aluno_Quando_Curso_Ativo()
    {
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();

        var aluno = new Aluno(alunoId, "Aluno Teste", "aluno@teste.com");
        var conteudo = new ConteudoProgramatico("Conteúdo", 1, DateTime.UtcNow);
        var curso = new Curso("Curso", "Descrição", 200m, 40, "Dev", "Aprender", "Pré requisitos", conteudo);

        var alunoRepository = new Mock<IAlunoRepository>();
        alunoRepository.Setup(r => r.ObterAlunoComMatriculas(alunoId)).ReturnsAsync(aluno);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Commit()).ReturnsAsync(true);
        alunoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWork.Object);

        var cursoRepository = new Mock<ICursoRepository>();
        cursoRepository.Setup(r => r.ObterPorId(cursoId)).ReturnsAsync(curso);

        var mediatorHandler = new Mock<IMediatorHandler>();

        var handler = new AlunoCommandHandler(alunoRepository.Object, cursoRepository.Object, mediatorHandler.Object);
        var command = new MatricularAlunoCommand(alunoId, cursoId);

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeTrue();
        aluno.Matriculas.Should().ContainSingle(m => m.CursoId == cursoId);
        alunoRepository.Verify(r => r.Atualizar(aluno), Times.Once);
        unitOfWork.Verify(u => u.Commit(), Times.Once);
        mediatorHandler.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Never);
    }

    [Fact]
    public async Task Nao_Deve_Matricular_Quando_Curso_Inexistente()
    {
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var aluno = new Aluno(alunoId, "Aluno Teste", "aluno@teste.com");

        var alunoRepository = new Mock<IAlunoRepository>();
        alunoRepository.Setup(r => r.ObterAlunoComMatriculas(alunoId)).ReturnsAsync(aluno);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Commit()).ReturnsAsync(true);
        alunoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWork.Object);

        var cursoRepository = new Mock<ICursoRepository>();
        cursoRepository.Setup(r => r.ObterPorId(cursoId)).ReturnsAsync((Curso?)null);

        var mediatorHandler = new Mock<IMediatorHandler>();

        var handler = new AlunoCommandHandler(alunoRepository.Object, cursoRepository.Object, mediatorHandler.Object);
        var command = new MatricularAlunoCommand(alunoId, cursoId);

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeFalse();
        alunoRepository.Verify(r => r.Atualizar(It.IsAny<Aluno>()), Times.Never);
        unitOfWork.Verify(u => u.Commit(), Times.Never);
        mediatorHandler.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
    }
}

