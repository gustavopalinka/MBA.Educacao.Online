using System;
using FluentAssertions;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.GestaoAlunos;

public class HistoricoAprendizadoTests
{
    [Fact]
    public void Deve_Registrar_Aulas_Sem_Duplicar()
    {
        var historico = new HistoricoAprendizado();
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();

        historico.RegistrarAulaConcluida(cursoId, aulaId);
        historico.RegistrarAulaConcluida(cursoId, aulaId);

        historico.ObterTotalAulasConcluidas().Should().Be(1);
    }

    [Fact]
    public void Deve_Obter_Data_De_Conclusao()
    {
        var historico = new HistoricoAprendizado();
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();

        historico.RegistrarAulaConcluida(cursoId, aulaId);

        var dataConclusao = historico.ObterDataConclusao(aulaId);
        dataConclusao.Should().HaveValue();
        historico.AulaJaConcluida(aulaId).Should().BeTrue();
    }
}


