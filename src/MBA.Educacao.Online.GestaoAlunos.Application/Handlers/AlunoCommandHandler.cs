using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MBA.Educacao.Online.Core.Mediator;
using MBA.Educacao.Online.GestaoAlunos.Application.Commands;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using MediatR;

namespace MBA.Educacao.Online.GestaoAlunos.Application.Handlers;

public class AlunoCommandHandler :
    CommandHandler,
    IRequestHandler<MatricularAlunoCommand, bool>,
    IRequestHandler<RegistrarProgressoCommand, bool>,
    IRequestHandler<FinalizarCursoCommand, bool>
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly ICursoRepository _cursoRepository;

    public AlunoCommandHandler(IAlunoRepository alunoRepository,
                               ICursoRepository cursoRepository,
                               IMediatorHandler mediatorHandler)
        : base(mediatorHandler)
    {
        _alunoRepository = alunoRepository;
        _cursoRepository = cursoRepository;
    }

    public async Task<bool> Handle(MatricularAlunoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EhValido())
        {
            await NotificarErros(request.ValidationResult);
            return false;
        }

        var aluno = await _alunoRepository.ObterAlunoComMatriculas(request.AlunoId);
        if (aluno is null)
        {
            await NotificarErro(request.MessageType, "Aluno não encontrado.");
            return false;
        }

        var curso = await _cursoRepository.ObterPorId(request.CursoId);
        if (curso is null || !curso.Ativo)
        {
            await NotificarErro(request.MessageType, "Curso não encontrado ou inativo.");
            return false;
        }

        var matriculaExistente = aluno.Matriculas.FirstOrDefault(m => m.CursoId == request.CursoId && m.Ativo);
        if (matriculaExistente is not null)
        {
            await NotificarErro(request.MessageType, "Aluno já matriculado neste curso.");
            return false;
        }

        aluno.MatricularEmCurso(request.CursoId);
        _alunoRepository.Atualizar(aluno);

        var sucesso = await _alunoRepository.UnitOfWork.Commit();
        if (!sucesso)
        {
            await NotificarErro(request.MessageType, "Falha ao registrar a matrícula.");
        }

        return sucesso;
    }

    public async Task<bool> Handle(RegistrarProgressoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EhValido())
        {
            await NotificarErros(request.ValidationResult);
            return false;
        }

        var aluno = await _alunoRepository.ObterAlunoComMatriculas(request.AlunoId);
        if (aluno is null)
        {
            await NotificarErro(request.MessageType, "Aluno não encontrado.");
            return false;
        }

        var matricula = aluno.Matriculas.FirstOrDefault(m =>
            m.CursoId == request.CursoId &&
            m.Status == StatusMatricula.Ativa);

        if (matricula is null)
        {
            await NotificarErro(request.MessageType, "Matrícula não encontrada ou inativa.");
            return false;
        }

        aluno.RegistrarProgresso(request.CursoId, request.AulaId);
        _alunoRepository.Atualizar(aluno);

        var sucesso = await _alunoRepository.UnitOfWork.Commit();
        if (!sucesso)
        {
            await NotificarErro(request.MessageType, "Falha ao registrar progresso.");
        }

        return sucesso;
    }

    public async Task<bool> Handle(FinalizarCursoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EhValido())
        {
            await NotificarErros(request.ValidationResult);
            return false;
        }

        var aluno = await _alunoRepository.ObterAlunoComMatriculas(request.AlunoId);
        if (aluno is null)
        {
            await NotificarErro(request.MessageType, "Aluno não encontrado.");
            return false;
        }

        var matricula = aluno.Matriculas.FirstOrDefault(m =>
            m.Id == request.MatriculaId &&
            m.CursoId == request.CursoId);

        if (matricula is null || matricula.Status != StatusMatricula.Ativa)
        {
            await NotificarErro(request.MessageType, "Matrícula inválida ou não ativa para finalização.");
            return false;
        }

        var curso = await _cursoRepository.ObterCursoComAulas(request.CursoId);
        if (curso is null)
        {
            await NotificarErro(request.MessageType, "Curso não encontrado.");
            return false;
        }

        var totalAulasCurso = curso.Aulas?.Count ?? 0;
        var aulasRealizadas = aluno.HistoricoAprendizado.ObterTotalAulasConcluidasPorCurso(request.CursoId);

        if (totalAulasCurso > 0 && aulasRealizadas < totalAulasCurso)
        {
            await NotificarErro(request.MessageType, "Existem aulas pendentes para conclusão.");
            return false;
        }

        aluno.ConcluirCurso(request.CursoId, request.MatriculaId);
        _alunoRepository.Atualizar(aluno);

        var sucesso = await _alunoRepository.UnitOfWork.Commit();
        if (!sucesso)
        {
            await NotificarErro(request.MessageType, "Falha ao finalizar o curso.");
        }

        return sucesso;
    }
}

