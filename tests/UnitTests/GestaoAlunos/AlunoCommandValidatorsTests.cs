using System;
using FluentAssertions;
using MBA.Educacao.Online.GestaoAlunos.Application.Commands;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.GestaoAlunos;

public class AlunoCommandValidatorsTests
{
    [Trait("Categoria", "GestaoAlunos - Validators")]
    [Fact(DisplayName = "MatricularAlunoCommand inválido quando campos vazios")]
    public void MatricularAlunoCommand_Deve_Falhar_Quando_Dados_Invalidos()
    {
        var command = new MatricularAlunoCommand(Guid.Empty, Guid.Empty);

        var valido = command.EhValido();

        valido.Should().BeFalse();
        command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("aluno"));
        command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("curso"));
    }

    [Trait("Categoria", "GestaoAlunos - Validators")]
    [Fact(DisplayName = "RegistrarProgressoCommand inválido quando campos vazios")]
    public void RegistrarProgressoCommand_Deve_Falhar_Quando_Dados_Invalidos()
    {
        var command = new RegistrarProgressoCommand(Guid.Empty, Guid.Empty, Guid.Empty);

        var valido = command.EhValido();

        valido.Should().BeFalse();
        command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("aluno"));
        command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("curso"));
        command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("aula"));
    }

    [Trait("Categoria", "GestaoAlunos - Validators")]
    [Fact(DisplayName = "FinalizarCursoCommand inválido quando campos vazios")]
    public void FinalizarCursoCommand_Deve_Falhar_Quando_Dados_Invalidos()
    {
        var command = new FinalizarCursoCommand(Guid.Empty, Guid.Empty, Guid.Empty);

        var valido = command.EhValido();

        valido.Should().BeFalse();
        command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("aluno"));
        command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("curso"));
        command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("matrícula"));
    }
}


