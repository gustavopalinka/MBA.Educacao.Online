using FluentValidation.Results;
using MBA.Educacao.Online.Core.Messages;
using MBA.Educacao.Online.Pagamentos.Application.Commands.Validators;

namespace MBA.Educacao.Online.Pagamentos.Application.Commands;

public class RealizarPagamentoCommand : Command
{
    public RealizarPagamentoCommand(Guid matriculaId, Guid alunoId, decimal valor,
                                    string numeroCartao, string nomeTitular, 
                                    string validade, string cvv)
    {
        MatriculaId = matriculaId;
        AlunoId = alunoId;
        Valor = valor;
        NumeroCartao = numeroCartao;
        NomeTitular = nomeTitular;
        Validade = validade;
        CVV = cvv;
    }

    public Guid MatriculaId { get; private set; }
    public Guid AlunoId { get; private set; }
    public decimal Valor { get; private set; }
    public string NumeroCartao { get; private set; }
    public string NomeTitular { get; private set; }
    public string Validade { get; private set; }
    public string CVV { get; private set; }

    public override bool EhValido()
    {
        ValidationResult = new RealizarPagamentoCommandValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}

