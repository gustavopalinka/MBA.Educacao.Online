using System;
using System.Linq;
using FluentAssertions;
using MBA.Educacao.Online.Core.DomainObjects;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.GestaoAlunos;

public class AlunoDomainTests
{
    [Fact]
    public void Deve_Matricular_Aluno_Em_Curso()
    {
        var aluno = new Aluno(Guid.NewGuid(), "Aluno Teste", "aluno@teste.com");
        var cursoId = Guid.NewGuid();

        aluno.MatricularEmCurso(cursoId);

        aluno.Matriculas.Should().ContainSingle(m => m.CursoId == cursoId);
        var matricula = aluno.Matriculas.First();
        matricula.Status.Should().Be(StatusMatricula.Pendente);
        matricula.Ativo.Should().BeTrue();
    }

    [Fact]
    public void Deve_Concluir_Curso_Gerando_Certificado()
    {
        var aluno = new Aluno(Guid.NewGuid(), "Aluno Teste", "aluno@teste.com");
        var cursoId = Guid.NewGuid();

        aluno.MatricularEmCurso(cursoId);
        var matricula = aluno.Matriculas.First();
        matricula.Ativar();

        aluno.ConcluirCurso(cursoId, matricula.Id);

        matricula.Status.Should().Be(StatusMatricula.Concluida);
        aluno.Certificados.Should().ContainSingle(c => c.CursoId == cursoId);
    }

    [Fact]
    public void Deve_Registrar_Progresso()
    {
        var aluno = new Aluno(Guid.NewGuid(), "Aluno Teste", "aluno@teste.com");
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();

        aluno.RegistrarProgresso(cursoId, aulaId);

        aluno.HistoricoAprendizado.ObterTotalAulasConcluidasPorCurso(cursoId).Should().Be(1);
        aluno.HistoricoAprendizado.AulaJaConcluida(aulaId).Should().BeTrue();
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Nome_Invalido()
    {
        Action act = () => new Aluno(Guid.NewGuid(), string.Empty, "aluno@teste.com");

        act.Should().Throw<DomainException>()
            .WithMessage("O nome do aluno não pode ser vazio");
    }
}


