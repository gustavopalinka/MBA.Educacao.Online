using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MBA.Educacao.Online.GestaoAlunos.Application.DTOs;
using MBA.Educacao.Online.GestaoAlunos.Application.Handlers;
using MBA.Educacao.Online.GestaoAlunos.Application.Queries;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using Moq;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.GestaoAlunos;

public class AlunoQueryHandlerTests
{
    [Trait("Categoria", "GestaoAlunos - QueryHandler")]
    [Fact(DisplayName = "Deve listar matrículas com nome do curso")]
    public async Task Deve_Listar_Matriculas_Com_Nome_Do_Curso()
    {
        var aluno = new Aluno(Guid.NewGuid(), "Aluno Teste", "aluno@teste.com");
        var cursoId = Guid.NewGuid();
        aluno.MatricularEmCurso(cursoId);

        var conteudo = new ConteudoProgramatico("Conteúdo", 4, DateTime.UtcNow);
        var curso = new Curso("Curso Teste", "Descrição", 100m, 10, "Dev", "Aprender", "Pré", conteudo);

        var alunoRepository = new Mock<IAlunoRepository>();
        alunoRepository.Setup(r => r.ObterAlunoComMatriculas(aluno.Id)).ReturnsAsync(aluno);

        var cursoRepository = new Mock<ICursoRepository>();
        cursoRepository.Setup(r => r.ObterPorId(cursoId)).ReturnsAsync(curso);

        var handler = new AlunoQueryHandler(alunoRepository.Object, cursoRepository.Object);

        var resultado = await handler.Handle(new ObterMatriculasAlunoQuery(aluno.Id), CancellationToken.None);

        resultado.Should().ContainSingle();
        resultado.First().NomeCurso.Should().Be("Curso Teste");
    }

    [Trait("Categoria", "GestaoAlunos - QueryHandler")]
    [Fact(DisplayName = "Deve listar certificados com nome do curso")]
    public async Task Deve_Listar_Certificados_Com_Nome_Do_Curso()
    {
        var aluno = new Aluno(Guid.NewGuid(), "Aluno Teste", "aluno@teste.com");
        var cursoId = Guid.NewGuid();
        aluno.MatricularEmCurso(cursoId);
        var matricula = aluno.Matriculas.First();
        matricula.Ativar();
        aluno.ConcluirCurso(cursoId, matricula.Id);

        var conteudo = new ConteudoProgramatico("Conteúdo", 4, DateTime.UtcNow);
        var curso = new Curso("Curso Teste", "Descrição", 100m, 10, "Dev", "Aprender", "Pré", conteudo);

        var alunoRepository = new Mock<IAlunoRepository>();
        alunoRepository.Setup(r => r.ObterAlunoComCertificados(aluno.Id)).ReturnsAsync(aluno);

        var cursoRepository = new Mock<ICursoRepository>();
        cursoRepository.Setup(r => r.ObterPorId(cursoId)).ReturnsAsync(curso);

        var handler = new AlunoQueryHandler(alunoRepository.Object, cursoRepository.Object);

        var resultado = await handler.Handle(new ObterCertificadosAlunoQuery(aluno.Id), CancellationToken.None);

        resultado.Should().ContainSingle();
        resultado.First().NomeCurso.Should().Be("Curso Teste");
    }

    [Trait("Categoria", "GestaoAlunos - QueryHandler")]
    [Fact(DisplayName = "Deve retornar progresso completo")]
    public async Task Deve_Retornar_Progresso_Com_Aulas()
    {
        var aluno = new Aluno(Guid.NewGuid(), "Aluno Teste", "aluno@teste.com");
        var cursoId = Guid.NewGuid();
        aluno.MatricularEmCurso(cursoId);
        var matricula = aluno.Matriculas.First();
        matricula.Ativar();

        var conteudo = new ConteudoProgramatico("Conteúdo", 4, DateTime.UtcNow);
        var curso = new Curso("Curso Teste", "Descrição", 100m, 10, "Dev", "Aprender", "Pré", conteudo);
        curso.AdicionarAula(new Aula("A1", "Aula 1", "Descrição", 1, curso.Id));
        curso.AdicionarAula(new Aula("A2", "Aula 2", "Descrição", 2, curso.Id));

        var aulaConcluida = curso.Aulas.First();
        aluno.RegistrarProgresso(cursoId, aulaConcluida.Id);

        var alunoRepository = new Mock<IAlunoRepository>();
        alunoRepository.Setup(r => r.ObterAlunoComMatriculas(aluno.Id)).ReturnsAsync(aluno);

        var cursoRepository = new Mock<ICursoRepository>();
        cursoRepository.Setup(r => r.ObterCursoComAulas(cursoId)).ReturnsAsync(curso);

        var handler = new AlunoQueryHandler(alunoRepository.Object, cursoRepository.Object);

        var resultado = await handler.Handle(new ObterProgressoCursoQuery(aluno.Id, cursoId), CancellationToken.None);

        resultado.Should().NotBeNull();
        resultado!.TotalAulas.Should().Be(2);
        resultado.AulasConcluidas.Should().Be(1);
        resultado.Aulas.Should().HaveCount(2);
        resultado.Aulas.First(a => a.AulaId == aulaConcluida.Id).Concluida.Should().BeTrue();
    }
}


