using System;
using System.Linq;
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

    [Fact]
    public async Task Nao_Deve_Matricular_Quando_Curso_Inativo()
    {
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();

        var aluno = new Aluno(alunoId, "Aluno Teste", "aluno@teste.com");
        var conteudo = new ConteudoProgramatico("Conteúdo", 1, DateTime.UtcNow);
        var curso = new Curso("Curso Inativo", "Descrição", 200m, 40, "Dev", "Aprender", "Pré requisitos", conteudo);
        curso.AlterarEstado(false);

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

        resultado.Should().BeFalse();
        alunoRepository.Verify(r => r.Atualizar(It.IsAny<Aluno>()), Times.Never);
        unitOfWork.Verify(u => u.Commit(), Times.Never);
        mediatorHandler.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Nao_Deve_Matricular_Quando_Ja_Matriculado()
    {
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();

        var aluno = new Aluno(alunoId, "Aluno Teste", "aluno@teste.com");
        aluno.MatricularEmCurso(cursoId);
        var matricula = aluno.Matriculas.First();
        matricula.Ativar();

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

        resultado.Should().BeFalse();
        alunoRepository.Verify(r => r.Atualizar(It.IsAny<Aluno>()), Times.Never);
        unitOfWork.Verify(u => u.Commit(), Times.Never);
        mediatorHandler.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Nao_Deve_Matricular_Quando_Comando_Invalido()
    {
        var alunoRepository = new Mock<IAlunoRepository>();
        var cursoRepository = new Mock<ICursoRepository>();
        var mediatorHandler = new Mock<IMediatorHandler>();

        var handler = new AlunoCommandHandler(alunoRepository.Object, cursoRepository.Object, mediatorHandler.Object);
        var command = new MatricularAlunoCommand(Guid.Empty, Guid.NewGuid());

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeFalse();
        alunoRepository.Verify(r => r.ObterAlunoComMatriculas(It.IsAny<Guid>()), Times.Never);
        cursoRepository.Verify(r => r.ObterPorId(It.IsAny<Guid>()), Times.Never);
        mediatorHandler.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Nao_Deve_Registrar_Progresso_Quando_Matricula_Inativa()
    {
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();

        var aluno = new Aluno(alunoId, "Aluno Progresso", "teste@teste.com");
        aluno.MatricularEmCurso(cursoId);

        var alunoRepository = new Mock<IAlunoRepository>();
        alunoRepository.Setup(r => r.ObterAlunoComMatriculas(alunoId)).ReturnsAsync(aluno);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Commit()).ReturnsAsync(true);
        alunoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWork.Object);

        var cursoRepository = new Mock<ICursoRepository>();

        var mediatorHandler = new Mock<IMediatorHandler>();

        var handler = new AlunoCommandHandler(alunoRepository.Object, cursoRepository.Object, mediatorHandler.Object);
        var command = new RegistrarProgressoCommand(alunoId, cursoId, aulaId);

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeFalse();
        alunoRepository.Verify(r => r.Atualizar(It.IsAny<Aluno>()), Times.Never);
        unitOfWork.Verify(u => u.Commit(), Times.Never);
        mediatorHandler.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Nao_Deve_Finalizar_Curso_Com_Aulas_Pendentes()
    {
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();

        var conteudo = new ConteudoProgramatico("Conteúdo", 1, DateTime.UtcNow);
        var curso = new Curso("Curso", "Descrição", 200m, 40, "Dev", "Aprender", "Pré requisitos", conteudo);
        var aula1 = new Aula("A1", "Aula 1", "Descricao", 1, curso.Id);
        var aula2 = new Aula("A2", "Aula 2", "Descricao", 2, curso.Id);
        curso.AdicionarAula(aula1);
        curso.AdicionarAula(aula2);

        var aluno = new Aluno(alunoId, "Aluno Teste", "aluno@teste.com");
        aluno.MatricularEmCurso(cursoId);
        var matricula = aluno.Matriculas.First();
        matricula.Ativar();

        aluno.RegistrarProgresso(cursoId, aula1.Id);

        var alunoRepository = new Mock<IAlunoRepository>();
        alunoRepository.Setup(r => r.ObterAlunoComMatriculas(alunoId)).ReturnsAsync(aluno);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Commit()).ReturnsAsync(true);
        alunoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWork.Object);

        var cursoRepository = new Mock<ICursoRepository>();
        cursoRepository.Setup(r => r.ObterCursoComAulas(cursoId)).ReturnsAsync(curso);

        var mediatorHandler = new Mock<IMediatorHandler>();

        var handler = new AlunoCommandHandler(alunoRepository.Object, cursoRepository.Object, mediatorHandler.Object);
        var command = new FinalizarCursoCommand(alunoId, cursoId, matricula.Id);

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeFalse();
        alunoRepository.Verify(r => r.Atualizar(It.IsAny<Aluno>()), Times.Never);
        unitOfWork.Verify(u => u.Commit(), Times.Never);
        mediatorHandler.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Deve_Registrar_Progresso_Quando_Matricula_Ativa()
    {
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();

        var aluno = new Aluno(alunoId, "Aluno Teste", "aluno@teste.com");
        aluno.MatricularEmCurso(cursoId);
        var matricula = aluno.Matriculas.First();
        matricula.Ativar();

        var alunoRepository = new Mock<IAlunoRepository>();
        alunoRepository.Setup(r => r.ObterAlunoComMatriculas(alunoId)).ReturnsAsync(aluno);
        alunoRepository.Setup(r => r.Atualizar(aluno));

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Commit()).ReturnsAsync(true);
        alunoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWork.Object);

        var cursoRepository = new Mock<ICursoRepository>();

        var mediatorHandler = new Mock<IMediatorHandler>();

        var handler = new AlunoCommandHandler(alunoRepository.Object, cursoRepository.Object, mediatorHandler.Object);
        var command = new RegistrarProgressoCommand(alunoId, cursoId, aulaId);

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeTrue();
        aluno.HistoricoAprendizado.ObterTotalAulasConcluidasPorCurso(cursoId).Should().Be(1);
        alunoRepository.Verify(r => r.Atualizar(aluno), Times.Once);
        unitOfWork.Verify(u => u.Commit(), Times.Once);
        mediatorHandler.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Never);
    }

    [Fact]
    public async Task Deve_Finalizar_Curso_Quando_Todas_Aulas_Completas()
    {
        var alunoId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();

        var conteudo = new ConteudoProgramatico("Conteúdo", 1, DateTime.UtcNow);
        var curso = new Curso("Curso", "Descrição", 200m, 40, "Dev", "Aprender", "Pré requisitos", conteudo);
        var aula1 = new Aula("A1", "Aula 1", "Descricao", 1, curso.Id);
        var aula2 = new Aula("A2", "Aula 2", "Descricao", 2, curso.Id);
        curso.AdicionarAula(aula1);
        curso.AdicionarAula(aula2);

        var aluno = new Aluno(alunoId, "Aluno Teste", "aluno@teste.com");
        aluno.MatricularEmCurso(cursoId);
        var matricula = aluno.Matriculas.First();
        matricula.Ativar();
        aluno.RegistrarProgresso(cursoId, aula1.Id);
        aluno.RegistrarProgresso(cursoId, aula2.Id);

        var alunoRepository = new Mock<IAlunoRepository>();
        alunoRepository.Setup(r => r.ObterAlunoComMatriculas(alunoId)).ReturnsAsync(aluno);
        alunoRepository.Setup(r => r.Atualizar(aluno));

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Commit()).ReturnsAsync(true);
        alunoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWork.Object);

        var cursoRepository = new Mock<ICursoRepository>();
        cursoRepository.Setup(r => r.ObterCursoComAulas(cursoId)).ReturnsAsync(curso);

        var mediatorHandler = new Mock<IMediatorHandler>();

        var handler = new AlunoCommandHandler(alunoRepository.Object, cursoRepository.Object, mediatorHandler.Object);
        var command = new FinalizarCursoCommand(alunoId, cursoId, matricula.Id);

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeTrue();
        matricula.Status.Should().Be(StatusMatricula.Concluida);
        aluno.Certificados.Should().ContainSingle(c => c.CursoId == cursoId);
        alunoRepository.Verify(r => r.Atualizar(aluno), Times.Once);
        unitOfWork.Verify(u => u.Commit(), Times.Once);
        mediatorHandler.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Never);
    }
}

