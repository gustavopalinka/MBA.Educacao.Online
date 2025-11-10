using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MBA.Educacao.Online.Core.Data;
using MBA.Educacao.Online.GestaoConteudo.Application.Commands;
using MBA.Educacao.Online.GestaoConteudo.Application.Handlers;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using Moq;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.GestaoConteudo;

public class CursoCommandHandlerTests
{
    [Trait("Categoria", "GestaoConteudo - CommandHandler")]
    [Fact(DisplayName = "Deve criar curso com comando válido")]
    public async Task Deve_Criar_Curso_Quando_Comando_Valido()
    {
        var cursoRepository = new Mock<ICursoRepository>();
        cursoRepository.Setup(r => r.Adicionar(It.IsAny<Curso>()));

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Commit()).ReturnsAsync(true);
        cursoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWork.Object);

        var handler = new CursoCommandHandler(cursoRepository.Object);
        var command = new CriarCursoCommand("Curso Teste", "Descrição", 100m, 10, "Dev", "Aprender", "Pré", "Conteúdo");

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeTrue();
        cursoRepository.Verify(r => r.Adicionar(It.IsAny<Curso>()), Times.Once);
        unitOfWork.Verify(u => u.Commit(), Times.Once);
    }

    [Trait("Categoria", "GestaoConteudo - CommandHandler")]
    [Fact(DisplayName = "Não deve criar curso com comando inválido")]
    public async Task Nao_Deve_Criar_Curso_Quando_Comando_Invalido()
    {
        var cursoRepository = new Mock<ICursoRepository>();
        var handler = new CursoCommandHandler(cursoRepository.Object);
        var command = new CriarCursoCommand(string.Empty, string.Empty, -1m, 0, string.Empty, string.Empty, string.Empty, string.Empty);

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeFalse();
        cursoRepository.Verify(r => r.Adicionar(It.IsAny<Curso>()), Times.Never);
    }

    [Trait("Categoria", "GestaoConteudo - CommandHandler")]
    [Fact(DisplayName = "Deve adicionar aula quando curso existe")]
    public async Task Deve_Adicionar_Aula_Quando_Curso_Existe()
    {
        var curso = new Curso("Curso Teste", "Descrição", 100m, 10, "Dev", "Aprender", "Pré", new ConteudoProgramatico("Conteúdo", 1, DateTime.UtcNow));
        var cursoRepository = new Mock<ICursoRepository>();
        cursoRepository.Setup(r => r.ObterPorId(curso.Id)).ReturnsAsync(curso);
        cursoRepository.Setup(r => r.Atualizar(curso));

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Commit()).ReturnsAsync(true);
        cursoRepository.SetupGet(r => r.UnitOfWork).Returns(unitOfWork.Object);

        var handler = new CursoCommandHandler(cursoRepository.Object);
        var command = new AdicionarAulaCommand(curso.Id, "A1", "Aula 1", "Descrição", 1);

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeTrue();
        curso.Aulas.Should().ContainSingle(a => a.Codigo == "A1");
        cursoRepository.Verify(r => r.Atualizar(curso), Times.Once);
        unitOfWork.Verify(u => u.Commit(), Times.Once);
    }

    [Trait("Categoria", "GestaoConteudo - CommandHandler")]
    [Fact(DisplayName = "Não deve adicionar aula quando curso não existe")]
    public async Task Nao_Deve_Adicionar_Aula_Quando_Curso_Nao_Existe()
    {
        var cursoRepository = new Mock<ICursoRepository>();
        cursoRepository.Setup(r => r.ObterPorId(It.IsAny<Guid>())).ReturnsAsync((Curso?)null);

        var handler = new CursoCommandHandler(cursoRepository.Object);
        var command = new AdicionarAulaCommand(Guid.NewGuid(), "A1", "Aula 1", "Descrição", 1);

        var resultado = await handler.Handle(command, CancellationToken.None);

        resultado.Should().BeFalse();
        cursoRepository.Verify(r => r.Atualizar(It.IsAny<Curso>()), Times.Never);
    }
}


