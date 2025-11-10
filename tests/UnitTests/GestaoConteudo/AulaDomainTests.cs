using System;
using FluentAssertions;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.GestaoConteudo;

public class AulaDomainTests
{
    [Fact]
    public void AtualizarInformacoes_Deve_Modificar_Dados_Da_Aula()
    {
        var aula = new Aula("A1", "Título", "Descrição", 1, Guid.NewGuid());

        aula.AtualizarInformacoes("A2", "Novo título", "Nova descrição", 2);

        aula.Codigo.Should().Be("A2");
        aula.Titulo.Should().Be("Novo título");
        aula.Descricao.Should().Be("Nova descrição");
        aula.Ordem.Should().Be(2);
    }

    [Fact]
    public void AlterarEstado_Deve_Modificar_Status()
    {
        var aula = new Aula("A1", "Título", "Descrição", 1, Guid.NewGuid());

        aula.AlterarEstado(false);

        aula.Ativo.Should().BeFalse();
    }
}


