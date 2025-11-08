using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MBA.Educacao.Online.GestaoAlunos.Application.Commands;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using MediatR;

namespace MBA.Educacao.Online.GestaoAlunos.Application.Handlers;

public class AlunoCommandHandler :
    IRequestHandler<MatricularAlunoCommand, bool>,
    IRequestHandler<RegistrarProgressoCommand, bool>,
    IRequestHandler<FinalizarCursoCommand, bool>
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly ICursoRepository _cursoRepository;

    public AlunoCommandHandler(IAlunoRepository alunoRepository,
                               ICursoRepository cursoRepository)
    {
        _alunoRepository = alunoRepository;
        _cursoRepository = cursoRepository;
    }

    public async Task<bool> Handle(MatricularAlunoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EhValido())
        {
            return false;
        }

        var aluno = await _alunoRepository.ObterAlunoComMatriculas(request.AlunoId);
        if (aluno is null)
        {
            return false;
        }

        var curso = await _cursoRepository.ObterPorId(request.CursoId);
        if (curso is null || !curso.Ativo)
        {
            return false;
        }

        var matriculaExistente = aluno.Matriculas.FirstOrDefault(m => m.CursoId == request.CursoId && m.Ativo);
        if (matriculaExistente is not null)
        {
            return false;
        }

        aluno.MatricularEmCurso(request.CursoId);

        return await _alunoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(RegistrarProgressoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EhValido())
        {
            return false;
        }

        var aluno = await _alunoRepository.ObterAlunoComMatriculas(request.AlunoId);
        if (aluno is null)
        {
            return false;
        }

        var matricula = aluno.Matriculas.FirstOrDefault(m =>
            m.CursoId == request.CursoId &&
            m.Status == StatusMatricula.Ativa);

        if (matricula is null)
        {
            return false;
        }

        aluno.RegistrarProgresso(request.CursoId, request.AulaId);

        return await _alunoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(FinalizarCursoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EhValido())
        {
            return false;
        }

        var aluno = await _alunoRepository.ObterAlunoComMatriculas(request.AlunoId);
        if (aluno is null)
        {
            return false;
        }

        var matricula = aluno.Matriculas.FirstOrDefault(m =>
            m.Id == request.MatriculaId &&
            m.CursoId == request.CursoId);

        if (matricula is null || matricula.Status != StatusMatricula.Ativa)
        {
            return false;
        }

        var curso = await _cursoRepository.ObterCursoComAulas(request.CursoId);
        if (curso is null)
        {
            return false;
        }

        var totalAulasCurso = curso.Aulas?.Count ?? 0;
        var aulasRealizadas = aluno.HistoricoAprendizado.ObterTotalAulasConcluidasPorCurso(request.CursoId);

        if (totalAulasCurso > 0 && aulasRealizadas < totalAulasCurso)
        {
            return false;
        }

        aluno.ConcluirCurso(request.CursoId, request.MatriculaId);

        return await _alunoRepository.UnitOfWork.Commit();
    }
}

