using MBA.Educacao.Online.Core.Messages;
using MBA.Educacao.Online.GestaoConteudo.Application.Commands;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using MediatR;

namespace MBA.Educacao.Online.GestaoConteudo.Application.Handlers;

/// <summary>
/// Handler para Commands relacionados a Curso
/// Implementa a lógica de negócio dos casos de uso
/// </summary>
public class CursoCommandHandler : 
    IRequestHandler<CriarCursoCommand, bool>,
    IRequestHandler<AdicionarAulaCommand, bool>
{
    private readonly ICursoRepository _cursoRepository;

    public CursoCommandHandler(ICursoRepository cursoRepository)
    {
        _cursoRepository = cursoRepository;
    }

    /// <summary>
    /// Handler para criar um novo curso
    /// Caso de Uso: Cadastro de Curso
    /// </summary>
    public async Task<bool> Handle(CriarCursoCommand request, CancellationToken cancellationToken)
    {
        // Validar comando
        if (!request.EhValido())
        {
            return false;
        }

        // Criar Value Object ConteudoProgramatico
        var conteudoProgramatico = new ConteudoProgramatico(
            request.ConteudoProgramatico, 
            revisao: 1, 
            DateTime.Now
        );

        // Criar Aggregate Root Curso
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

        // Persistir
        _cursoRepository.Adicionar(curso);
        
        // Commit (Unit of Work)
        return await _cursoRepository.UnitOfWork.Commit();
    }

    /// <summary>
    /// Handler para adicionar uma aula a um curso
    /// Caso de Uso: Cadastro de Aula
    /// </summary>
    public async Task<bool> Handle(AdicionarAulaCommand request, CancellationToken cancellationToken)
    {
        // Validar comando
        if (!request.EhValido())
        {
            return false;
        }

        // Buscar o curso (Aggregate Root)
        var curso = await _cursoRepository.ObterPorId(request.CursoId);
        
        if (curso == null)
        {
            return false;
        }

        // Criar a aula
        var aula = new Aula(
            request.Codigo,
            request.Titulo,
            request.Descricao,
            request.Ordem,
            request.CursoId
        );

        // Adicionar aula ao curso (comportamento do aggregate)
        curso.AdicionarAula(aula);

        // Atualizar curso
        _cursoRepository.Atualizar(curso);

        // Commit (Unit of Work)
        return await _cursoRepository.UnitOfWork.Commit();
    }
}

