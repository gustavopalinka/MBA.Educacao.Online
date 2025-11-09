using MBA.Educacao.Online.Core.Extensions;
using MBA.Educacao.Online.Core.Mediator;
using MBA.Educacao.Online.GestaoAlunos.Application.Commands;
using MBA.Educacao.Online.GestaoAlunos.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MBA.Educacao.Online.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class AlunosController : MainController
{
    private readonly IMediator _mediator;

    public AlunosController(IMediator mediator,
                            DomainNotificationHandler notifications,
                            IMediatorHandler mediatorHandler)
        : base(notifications, mediatorHandler)
    {
        _mediator = mediator;
    }

    [HttpGet("matriculas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterMatriculas()
    {
        var alunoId = User.FindFirst(ClaimTypes.Name)?.Value.ToGuid();

        if (alunoId == null || alunoId == Guid.Empty)
            return Unauthorized("Usuário não autenticado");

        var query = new ObterMatriculasAlunoQuery(alunoId.Value);
        var matriculas = await _mediator.Send(query);

        return CustomResponse(matriculas);
    }

    [HttpPost("matriculas")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MatricularEmCurso([FromBody] MatricularRequest request)
    {
        if (!ModelState.IsValid)
        {
            await NotificarErroModelInvalido();
            return CustomResponse();
        }

        var alunoId = User.FindFirst(ClaimTypes.Name)?.Value.ToGuid();

        if (alunoId == null || alunoId == Guid.Empty)
            return Unauthorized("Usuário não autenticado");

        var command = new MatricularAlunoCommand(alunoId.Value, request.CursoId);
        var resultado = await MediatorHandler.EnviarComando(command);

        if (!resultado)
            return CustomResponse();

        return Created(string.Empty, new { message = "Matrícula realizada com sucesso. Aguardando pagamento." });
    }

    [HttpPost("progresso")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegistrarProgresso([FromBody] ProgressoRequest request)
    {
        if (!ModelState.IsValid)
        {
            await NotificarErroModelInvalido();
            return CustomResponse();
        }

        var alunoId = User.FindFirst(ClaimTypes.Name)?.Value.ToGuid();

        if (alunoId == null || alunoId == Guid.Empty)
            return Unauthorized("Usuário não autenticado");

        var command = new RegistrarProgressoCommand(alunoId.Value, request.CursoId, request.AulaId);
        var resultado = await MediatorHandler.EnviarComando(command);

        if (!resultado)
            return CustomResponse();

        return CustomResponse(new { message = "Progresso registrado com sucesso" });
    }

    [HttpGet("{cursoId:guid}/progresso")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterProgresso(Guid cursoId)
    {
        var alunoId = User.FindFirst(ClaimTypes.Name)?.Value.ToGuid();

        if (alunoId == null || alunoId == Guid.Empty)
            return Unauthorized("Usuário não autenticado");

        var query = new ObterProgressoCursoQuery(alunoId.Value, cursoId);
        var progresso = await _mediator.Send(query);

        return progresso is null
            ? NotFound("Progresso não encontrado para o curso informado.")
            : CustomResponse(progresso);
    }

    [HttpGet("certificados")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterCertificados()
    {
        var alunoId = User.FindFirst(ClaimTypes.Name)?.Value.ToGuid();

        if (alunoId == null || alunoId == Guid.Empty)
            return Unauthorized("Usuário não autenticado");

        var query = new ObterCertificadosAlunoQuery(alunoId.Value);
        var certificados = await _mediator.Send(query);

        return CustomResponse(certificados);
    }

    [HttpPost("finalizar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FinalizarCurso([FromBody] FinalizarCursoRequest request)
    {
        if (!ModelState.IsValid)
        {
            await NotificarErroModelInvalido();
            return CustomResponse();
        }

        var alunoId = User.FindFirst(ClaimTypes.Name)?.Value.ToGuid();

        if (alunoId == null || alunoId == Guid.Empty)
            return Unauthorized("Usuário não autenticado");

        var command = new FinalizarCursoCommand(alunoId.Value, request.CursoId, request.MatriculaId);
        var resultado = await MediatorHandler.EnviarComando(command);

        if (!resultado)
            return CustomResponse();

        return CustomResponse(new { message = "Curso finalizado com sucesso! Certificado gerado." });
    }
}

public record MatricularRequest(Guid CursoId);
public record ProgressoRequest(Guid CursoId, Guid AulaId);
public record FinalizarCursoRequest(Guid CursoId, Guid MatriculaId);