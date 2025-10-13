using MBA.Educacao.Online.Core.Messages;
using FluentValidation.Results;

namespace MBA.Educacao.Online.GestaoConteudo.Application.Commands;

/// <summary>
/// Command para adicionar uma aula a um curso existente
/// Caso de Uso: Cadastro de Aula (do PDF de requisitos)
/// </summary>
public class AdicionarAulaCommand : Command
{
    public AdicionarAulaCommand(Guid cursoId, string codigo, string titulo, 
                                string descricao, int ordem)
    {
        CursoId = cursoId;
        Codigo = codigo;
        Titulo = titulo;
        Descricao = descricao;
        Ordem = ordem;
    }

    public Guid CursoId { get; private set; }
    public string Codigo { get; private set; }
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public int Ordem { get; private set; }

    public override bool EhValido()
    {
        ValidationResult = new AdicionarAulaCommandValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}

