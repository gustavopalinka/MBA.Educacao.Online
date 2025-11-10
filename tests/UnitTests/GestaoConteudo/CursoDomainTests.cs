using System;
using FluentAssertions;
using MBA.Educacao.Online.Core.DomainObjects;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.GestaoConteudo;

public class CursoDomainTests
{
    [Fact]
    public void Deve_Criar_Curso_Com_Aulas()
    {
        var conteudo = new ConteudoProgramatico("Conteúdo", 10, DateTime.UtcNow);

        var curso = new Curso("Curso Teste", "Descrição", 100m, 20, "Dev", "Aprender", "Pré", conteudo);
        var aula = new Aula("A1", "Aula 1", "Descrição Aula", 1, curso.Id);

        curso.AdicionarAula(aula);

        curso.Aulas.Should().ContainSingle(a => a.Id == aula.Id);
        curso.Ativo.Should().BeTrue();
    }

    [Fact]
    public void Deve_Remover_Aula()
    {
        var conteudo = new ConteudoProgramatico("Conteúdo", 10, DateTime.UtcNow);
        var curso = new Curso("Curso Teste", "Descrição", 100m, 20, "Dev", "Aprender", "Pré", conteudo);
        var aula = new Aula("A1", "Aula 1", "Descrição Aula", 1, curso.Id);
        curso.AdicionarAula(aula);

        curso.RemoverAula(aula);

        curso.Aulas.Should().BeEmpty();
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Valor_Negativo()
    {
        var conteudo = new ConteudoProgramatico("Conteúdo", 10, DateTime.UtcNow);

        Action act = () => new Curso("Curso Teste", "Descrição", -1m, 20, "Dev", "Aprender", "Pré", conteudo);

        act.Should().Throw<DomainException>()
            .WithMessage("O valor do curso não pode ser negativo");
    }
}


