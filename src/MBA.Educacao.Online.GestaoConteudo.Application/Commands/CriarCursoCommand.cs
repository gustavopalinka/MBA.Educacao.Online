using MBA.Educacao.Online.Core.Messages;
using FluentValidation.Results;

namespace MBA.Educacao.Online.GestaoConteudo.Application.Commands;

/// <summary>
/// Command para criar um novo curso
/// Caso de Uso: Cadastro de Curso (do PDF de requisitos)
/// </summary>
public class CriarCursoCommand : Command
{
    public CriarCursoCommand(string nome, string descricao, decimal valor, int cargaHoraria,
                             string publicoAlvo, string objetivo, string requisitos,
                             string conteudoProgramatico)
    {
        Nome = nome;
        Descricao = descricao;
        Valor = valor;
        CargaHoraria = cargaHoraria;
        PublicoAlvo = publicoAlvo;
        Objetivo = objetivo;
        Requisitos = requisitos;
        ConteudoProgramatico = conteudoProgramatico;
    }

    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public decimal Valor { get; private set; }
    public int CargaHoraria { get; private set; }
    public string PublicoAlvo { get; private set; }
    public string Objetivo { get; private set; }
    public string Requisitos { get; private set; }
    public string ConteudoProgramatico { get; private set; }

    public override bool EhValido()
    {
        ValidationResult = new CriarCursoCommandValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}

