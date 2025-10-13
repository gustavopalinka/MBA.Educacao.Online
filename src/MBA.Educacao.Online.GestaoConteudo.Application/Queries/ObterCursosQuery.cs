using MBA.Educacao.Online.GestaoConteudo.Application.DTOs;
using MediatR;

namespace MBA.Educacao.Online.GestaoConteudo.Application.Queries;

/// <summary>
/// Query para obter todos os cursos ativos
/// Lado de leitura do CQRS
/// </summary>
public class ObterCursosAtivoQuery : IRequest<IEnumerable<CursoDTO>>
{
}

/// <summary>
/// Query para obter um curso por ID com suas aulas
/// </summary>
public class ObterCursoPorIdQuery : IRequest<CursoDTO?>
{
    public ObterCursoPorIdQuery(Guid cursoId)
    {
        CursoId = cursoId;
    }

    public Guid CursoId { get; private set; }
}

