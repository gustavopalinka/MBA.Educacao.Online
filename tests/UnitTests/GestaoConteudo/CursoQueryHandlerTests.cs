using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MBA.Educacao.Online.GestaoConteudo.Application.Handlers;
using MBA.Educacao.Online.GestaoConteudo.Application.Queries;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using Moq;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.GestaoConteudo;

public class CursoQueryHandlerTests
{
    [Fact]
    public async Task Deve_Retornar_Cursos_Ativos()
    {
        var conteudo = new ConteudoProgramatico("Conteúdo", 1, DateTime.UtcNow);
        var curso1 = new Curso("Curso 1", "Descricao", 100m, 20, "Dev", "Aprender", "Pré", conteudo);
        var curso2 = new Curso("Curso 2", "Descricao", 200m, 30, "Dev", "Aprender", "Pré", conteudo);
        var cursos = new[] { curso1, curso2 };

        var cursoRepository = new Mock<ICursoRepository>();
        cursoRepository.Setup(r => r.ObterCursosAtivos()).ReturnsAsync(cursos);

        var handler = new CursoQueryHandler(cursoRepository.Object);

        var resultado = await handler.Handle(new ObterCursosAtivoQuery(), CancellationToken.None);

        resultado.Should().HaveCount(2);
        resultado.Select(c => c.Nome).Should().Contain(new[] { "Curso 1", "Curso 2" });
    }

    [Fact]
    public async Task Deve_Retornar_Curso_Com_Aulas()
    {
        var conteudo = new ConteudoProgramatico("Conteúdo", 1, DateTime.UtcNow);
        var curso = new Curso("Curso 1", "Descricao", 100m, 20, "Dev", "Aprender", "Pré", conteudo);
        var aula1 = new Aula("A1", "Aula 1", "Descricao", 1, curso.Id);
        var aula2 = new Aula("A2", "Aula 2", "Descricao", 2, curso.Id);
        curso.AdicionarAula(aula1);
        curso.AdicionarAula(aula2);

        var cursoRepository = new Mock<ICursoRepository>();
        cursoRepository.Setup(r => r.ObterCursoComAulas(curso.Id)).ReturnsAsync(curso);

        var handler = new CursoQueryHandler(cursoRepository.Object);

        var resultado = await handler.Handle(new ObterCursoPorIdQuery(curso.Id), CancellationToken.None);

        resultado.Should().NotBeNull();
        resultado!.Aulas.Should().HaveCount(2);
        resultado.Aulas.Select(a => a.Codigo).Should().Contain(new[] { "A1", "A2" });
    }

    [Fact]
    public async Task Deve_Retornar_Null_Quando_Curso_Nao_Encontrado()
    {
        var cursoRepository = new Mock<ICursoRepository>();
        cursoRepository.Setup(r => r.ObterCursoComAulas(It.IsAny<Guid>())).ReturnsAsync((Curso?)null);

        var handler = new CursoQueryHandler(cursoRepository.Object);

        var resultado = await handler.Handle(new ObterCursoPorIdQuery(Guid.NewGuid()), CancellationToken.None);

        resultado.Should().BeNull();
    }
}


