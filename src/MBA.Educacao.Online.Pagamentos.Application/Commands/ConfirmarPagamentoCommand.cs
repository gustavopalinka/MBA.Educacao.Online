using FluentValidation.Results;
using MBA.Educacao.Online.Core.Messages;
using MBA.Educacao.Online.Pagamentos.Application.Commands.Validators;

namespace MBA.Educacao.Online.Pagamentos.Application.Commands;

public class ConfirmarPagamentoCommand : Command
{
    public ConfirmarPagamentoCommand(Guid pagamentoId)
    {
        PagamentoId = pagamentoId;
    }

    public Guid PagamentoId { get; private set; }

    public override bool EhValido()
    {
        ValidationResult = new ConfirmarPagamentoCommandValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}

