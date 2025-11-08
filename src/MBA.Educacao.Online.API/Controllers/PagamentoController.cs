using System.Security.Claims;
using MBA.Educacao.Online.Core.Extensions;
using MBA.Educacao.Online.Pagamentos.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Educacao.Online.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagamentoController : ControllerBase
{
    private readonly IMediator _mediator;

    public PagamentoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Aluno")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RealizarPagamento([FromBody] RealizarPagamentoRequest request)
    {
        var alunoId = User.FindFirst(ClaimTypes.Name)?.Value.ToGuid();

        if (alunoId is null || alunoId == Guid.Empty)
        {
            return Unauthorized("Usuário não autenticado");
        }

        var command = new RealizarPagamentoCommand(
            request.MatriculaId,
            alunoId.Value,
            request.Valor,
            request.NumeroCartao,
            request.NomeTitular,
            request.Validade,
            request.CVV);

        var resultado = await _mediator.Send(command);

        if (!resultado)
        {
            return BadRequest("Não foi possível processar o pagamento.");
        }

        return Ok("Pagamento registrado. Aguarde confirmação.");
    }

    [HttpPost("{pagamentoId:guid}/confirmar")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmarPagamento(Guid pagamentoId)
    {
        var command = new ConfirmarPagamentoCommand(pagamentoId);
        var resultado = await _mediator.Send(command);

        if (!resultado)
        {
            return NotFound("Pagamento não encontrado ou já confirmado.");
        }

        return Ok("Pagamento confirmado com sucesso.");
    }
}

public record RealizarPagamentoRequest(
    Guid MatriculaId,
    decimal Valor,
    string NumeroCartao,
    string NomeTitular,
    string Validade,
    string CVV);

