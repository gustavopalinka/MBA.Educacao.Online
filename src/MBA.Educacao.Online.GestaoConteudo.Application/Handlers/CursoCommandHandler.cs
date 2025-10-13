using MBA.Educacao.Online.GestaoConteudo.Application.Commands;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using MediatR;

namespace MBA.Educacao.Online.GestaoConteudo.Application.Handlers;

public class CursoCommandHandler : 
    IRequestHandler<CriarCursoCommand, bool>,
    IRequestHandler<AdicionarAulaCommand, bool>
{
    private readonly ICursoRepository _cursoRepository;

    public CursoCommandHandler(ICursoRepository cursoRepository)
    {
        _cursoRepository = cursoRepository;
    }

    public async Task<bool> Handle(CriarCursoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EhValido())
        {
            return false;
        }

        var conteudoProgramatico = new ConteudoProgramatico(
            request.ConteudoProgramatico, 
            revisao: 1, 
            DateTime.Now
        );

        var curso = new Curso(
            request.Nome,
            request.Descricao,
            request.Valor,
            request.CargaHoraria,
            request.PublicoAlvo,
            request.Objetivo,
            request.Requisitos,
            conteudoProgramatico
        );

        _cursoRepository.Adicionar(curso);
        return await _cursoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(AdicionarAulaCommand request, CancellationToken cancellationToken)
    {
        if (!request.EhValido())
        {
            return false;
        }

        var curso = await _cursoRepository.ObterPorId(request.CursoId);
        
        if (curso == null)
        {
            return false;
        }

        var aula = new Aula(
            request.Codigo,
            request.Titulo,
            request.Descricao,
            request.Ordem,
            request.CursoId
        );

        curso.AdicionarAula(aula);
        _cursoRepository.Atualizar(curso);
        return await _cursoRepository.UnitOfWork.Commit();
    }
}