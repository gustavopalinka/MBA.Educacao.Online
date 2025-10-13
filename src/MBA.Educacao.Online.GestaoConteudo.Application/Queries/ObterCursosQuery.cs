using MBA.Educacao.Online.GestaoConteudo.Application.DTOs;
using MediatR;

namespace MBA.Educacao.Online.GestaoConteudo.Application.Queries;

public class ObterCursosAtivoQuery : IRequest<IEnumerable<CursoDTO>>
{
}

public class ObterCursoPorIdQuery : IRequest<CursoDTO?>
{
    public ObterCursoPorIdQuery(Guid cursoId)
    {
        CursoId = cursoId;
    }

    public Guid CursoId { get; private set; }
}