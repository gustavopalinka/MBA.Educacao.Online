using System;
using FluentAssertions;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.GestaoConteudo;

public class ConteudoProgramaticoTests
{
    [Trait("Categoria", "GestaoConteudo - Dominio")]
    [Fact(DisplayName = "Nova revisão deve incrementar versão")]
    public void NovaRevisao_Deve_Gerar_Conteudo_Com_Revisao_Incrementada()
    {
        var conteudo = new ConteudoProgramatico("Conteúdo inicial", 1, DateTime.UtcNow);

        var novaRevisao = conteudo.NovaRevisao("Conteúdo revisado");

        novaRevisao.Revisao.Should().Be(conteudo.Revisao + 1);
        novaRevisao.ConteudoDescricao.Should().Be("Conteúdo revisado");
    }

    [Trait("Categoria", "GestaoConteudo - Dominio")]
    [Fact(DisplayName = "Equals deve considerar descrição, revisão e data")]
    public void Equals_Deve_Considerar_Descricao_Revisao_E_Data()
    {
        var data = DateTime.UtcNow;
        var conteudo1 = new ConteudoProgramatico("Conteúdo", 1, data);
        var conteudo2 = new ConteudoProgramatico("Conteúdo", 1, data);

        conteudo1.Should().Be(conteudo2);
        conteudo1.GetHashCode().Should().Be(conteudo2.GetHashCode());
    }
}


