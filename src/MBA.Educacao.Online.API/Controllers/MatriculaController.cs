using MBA.Educacao.Online.Core.Extensions;
using MBA.Educacao.Online.GestaoAlunos.Application.Commands;
using MBA.Educacao.Online.GestaoAlunos.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MBA.Educacao.Online.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MatriculaController : ControllerBase
{
    private readonly IMediator _mediator;

    public MatriculaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterMatriculas()
    {
        var alunoId = User.FindFirst(ClaimTypes.Name)?.Value.ToGuid();

        if (alunoId == null || alunoId == Guid.Empty)
            return Unauthorized("Usuário não autenticado");

        var query = new ObterMatriculasAlunoQuery(alunoId.Value);
        var matriculas = await _mediator.Send(query);

        return Ok(matriculas);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MatricularEmCurso([FromBody] MatricularRequest request)
    {
        var alunoId = User.FindFirst(ClaimTypes.Name)?.Value.ToGuid();

        if (alunoId == null || alunoId == Guid.Empty)
            return Unauthorized("Usuário não autenticado");

        var command = new MatricularAlunoCommand(alunoId.Value, request.CursoId);
        var resultado = await _mediator.Send(command);

        if (!resultado)
            return BadRequest("Erro ao realizar matrícula");

        return Created("", "Matrícula realizada com sucesso. Aguardando pagamento.");
    }

    [HttpPost("progresso")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegistrarProgresso([FromBody] ProgressoRequest request)
    {
        var alunoId = User.FindFirst(ClaimTypes.Name)?.Value.ToGuid();

        if (alunoId == null || alunoId == Guid.Empty)
            return Unauthorized("Usuário não autenticado");

        var command = new RegistrarProgressoCommand(alunoId.Value, request.CursoId, request.AulaId);
        var resultado = await _mediator.Send(command);

        if (!resultado)
            return BadRequest("Erro ao registrar progresso");

        return Ok("Progresso registrado com sucesso");
    }

    [HttpGet("progresso/{cursoId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterProgresso(Guid cursoId)
    {
        var alunoId = User.FindFirst(ClaimTypes.Name)?.Value.ToGuid();

        if (alunoId == null || alunoId == Guid.Empty)
            return Unauthorized("Usuário não autenticado");

        var query = new ObterProgressoCursoQuery(alunoId.Value, cursoId);
        var progresso = await _mediator.Send(query);

        return Ok(progresso);
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

        return Ok(certificados);
    }

    [HttpPost("finalizar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FinalizarCurso([FromBody] FinalizarCursoRequest request)
    {
        var alunoId = User.FindFirst(ClaimTypes.Name)?.Value.ToGuid();

        if (alunoId == null || alunoId == Guid.Empty)
            return Unauthorized("Usuário não autenticado");

        var command = new FinalizarCursoCommand(alunoId.Value, request.CursoId, request.MatriculaId);
        var resultado = await _mediator.Send(command);

        if (!resultado)
            return BadRequest("Erro ao finalizar curso. Verifique se todas as aulas foram concluídas.");

        return Ok("Curso finalizado com sucesso! Certificado gerado.");
    }
}

public record MatricularRequest(Guid CursoId);
public record ProgressoRequest(Guid CursoId, Guid AulaId);
public record FinalizarCursoRequest(Guid CursoId, Guid MatriculaId);