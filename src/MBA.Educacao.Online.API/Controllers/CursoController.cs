using MBA.Educacao.Online.GestaoConteudo.Application.Commands;
using MBA.Educacao.Online.GestaoConteudo.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Educacao.Online.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CursoController : ControllerBase
{
    private readonly IMediator _mediator;

    public CursoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterCursosAtivos()
    {
        var query = new ObterCursosAtivoQuery();
        var cursos = await _mediator.Send(query);
        return Ok(cursos);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterCursoPorId(Guid id)
    {
        var query = new ObterCursoPorIdQuery(id);
        var curso = await _mediator.Send(query);

        if (curso == null)
            return NotFound("Curso não encontrado");

        return Ok(curso);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CriarCurso([FromBody] CriarCursoCommand command)
    {
        var resultado = await _mediator.Send(command);

        if (!resultado)
            return BadRequest("Erro ao criar curso");

        return CreatedAtAction(nameof(ObterCursoPorId), new { id = command.AggregateId }, command);
    }

    [HttpPost("{cursoId:guid}/aulas")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AdicionarAula(Guid cursoId, [FromBody] AdicionarAulaRequest request)
    {
        var command = new AdicionarAulaCommand(cursoId, request.Codigo, request.Titulo, 
                                               request.Descricao, request.Ordem);
        var resultado = await _mediator.Send(command);

        if (!resultado)
            return BadRequest("Erro ao adicionar aula");

        return Created("", "Aula adicionada com sucesso");
    }
}

public record AdicionarAulaRequest(string Codigo, string Titulo, string Descricao, int Ordem);