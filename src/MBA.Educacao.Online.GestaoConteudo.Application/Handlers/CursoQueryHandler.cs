using MBA.Educacao.Online.GestaoConteudo.Application.DTOs;
using MBA.Educacao.Online.GestaoConteudo.Application.Queries;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using MediatR;

namespace MBA.Educacao.Online.GestaoConteudo.Application.Handlers;

public class CursoQueryHandler :
    IRequestHandler<ObterCursosAtivoQuery, IEnumerable<CursoDTO>>,
    IRequestHandler<ObterCursoPorIdQuery, CursoDTO?>
{
    private readonly ICursoRepository _cursoRepository;

    public CursoQueryHandler(ICursoRepository cursoRepository)
    {
        _cursoRepository = cursoRepository;
    }

    public async Task<IEnumerable<CursoDTO>> Handle(ObterCursosAtivoQuery request, CancellationToken cancellationToken)
    {
        var cursos = await _cursoRepository.ObterCursosAtivos();

        return cursos.Select(c => new CursoDTO
        {
            Id = c.Id,
            Nome = c.Nome,
            Descricao = c.Descricao,
            Valor = c.Valor,
            CargaHoraria = c.CargaHoraria,
            PublicoAlvo = c.PublicoAlvo,
            Objetivo = c.Objetivo,
            Requisitos = c.Requisitos,
            ConteudoProgramatico = c.ConteudoProgramatico.ConteudoDescricao,
            Ativo = c.Ativo,
            DataCadastro = c.DataCadastro
        }).ToList();
    }

    public async Task<CursoDTO?> Handle(ObterCursoPorIdQuery request, CancellationToken cancellationToken)
    {
        var curso = await _cursoRepository.ObterCursoComAulas(request.CursoId);

        if (curso == null)
            return null;

        return new CursoDTO
        {
            Id = curso.Id,
            Nome = curso.Nome,
            Descricao = curso.Descricao,
            Valor = curso.Valor,
            CargaHoraria = curso.CargaHoraria,
            PublicoAlvo = curso.PublicoAlvo,
            Objetivo = curso.Objetivo,
            Requisitos = curso.Requisitos,
            ConteudoProgramatico = curso.ConteudoProgramatico.ConteudoDescricao,
            Ativo = curso.Ativo,
            DataCadastro = curso.DataCadastro,
            Aulas = curso.Aulas.Select(a => new AulaDTO
            {
                Id = a.Id,
                Codigo = a.Codigo,
                Titulo = a.Titulo,
                Descricao = a.Descricao,
                Ordem = a.Ordem,
                Ativo = a.Ativo,
                DataCadastro = a.DataCadastro
            }).ToList()
        };
    }
}