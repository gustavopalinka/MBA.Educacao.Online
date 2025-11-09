using System.Security.Claims;
using MBA.Educacao.Online.Core.Extensions;
using MBA.Educacao.Online.Core.Mediator;
using MBA.Educacao.Online.Pagamentos.Application.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MBA.Educacao.Online.API.Controllers;

[Route("api/[controller]")]
public class PagamentoController : MainController
{
    public PagamentoController(DomainNotificationHandler notifications,
                               IMediatorHandler mediatorHandler)
        : base(notifications, mediatorHandler)
    { }

    [HttpPost]
    [Authorize(Roles = "Aluno")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RealizarPagamento([FromBody] RealizarPagamentoRequest request)
    {
        if (!ModelState.IsValid)
        {
            await NotificarErroModelInvalido();
            return CustomResponse();
        }

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

        var resultado = await MediatorHandler.EnviarComando(command);

        if (!resultado)
        {
            return CustomResponse();
        }

        return CustomResponse(new { message = "Pagamento registrado. Aguarde confirmação." });
    }

    [HttpPost("{pagamentoId:guid}/confirmar")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmarPagamento(Guid pagamentoId)
    {
        var command = new ConfirmarPagamentoCommand(pagamentoId);
        var resultado = await MediatorHandler.EnviarComando(command);

        if (!resultado)
        {
            return CustomResponse();
        }

        return CustomResponse(new { message = "Pagamento confirmado com sucesso." });
    }
}

public record RealizarPagamentoRequest(
    Guid MatriculaId,
    decimal Valor,
    string NumeroCartao,
    string NomeTitular,
    string Validade,
    string CVV);

