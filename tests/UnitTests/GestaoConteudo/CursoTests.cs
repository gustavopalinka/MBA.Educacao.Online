using FluentAssertions;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using MBA.Educacao.Online.Core.DomainObjects;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.GestaoConteudo;

public class CursoTests
{
    [Fact]
    public void Deve_Criar_Curso_Quando_Dados_Validos()
    {
        var conteudo = new ConteudoProgramatico("Conteúdo completo", 1, DateTime.UtcNow);

        var curso = new Curso("Curso Teste", "Descrição do curso", 100m, 20, "Desenvolvedores",
            "Aprender testes", "Conhecimentos básicos", conteudo);

        curso.EhValido().Should().BeTrue();
        curso.Aulas.Should().BeEmpty();
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Valor_Negativo()
    {
        var conteudo = new ConteudoProgramatico("Conteúdo", 1, DateTime.UtcNow);

        var acao = () => new Curso("Curso Teste", "Descrição do curso", -1m, 20, "Dev", "Objetivo", "Requisitos", conteudo);

        acao.Should().Throw<DomainException>()
            .WithMessage("O valor do curso não pode ser negativo");
    }
}

