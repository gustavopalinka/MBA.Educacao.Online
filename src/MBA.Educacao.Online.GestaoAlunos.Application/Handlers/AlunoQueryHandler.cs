using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MBA.Educacao.Online.GestaoAlunos.Application.DTOs;
using MBA.Educacao.Online.GestaoAlunos.Application.Queries;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using MediatR;

namespace MBA.Educacao.Online.GestaoAlunos.Application.Handlers;

public class AlunoQueryHandler :
    IRequestHandler<ObterMatriculasAlunoQuery, IEnumerable<MatriculaDTO>>,
    IRequestHandler<ObterCertificadosAlunoQuery, IEnumerable<CertificadoDTO>>,
    IRequestHandler<ObterProgressoCursoQuery, ProgressoCursoDTO?>
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly ICursoRepository _cursoRepository;

    public AlunoQueryHandler(IAlunoRepository alunoRepository,
                             ICursoRepository cursoRepository)
    {
        _alunoRepository = alunoRepository;
        _cursoRepository = cursoRepository;
    }

    public async Task<IEnumerable<MatriculaDTO>> Handle(ObterMatriculasAlunoQuery request, CancellationToken cancellationToken)
    {
        var aluno = await _alunoRepository.ObterAlunoComMatriculas(request.AlunoId);

        if (aluno is null)
        {
            return Enumerable.Empty<MatriculaDTO>();
        }

        return aluno.Matriculas
            .OrderByDescending(m => m.DataMatricula)
            .Select(m => new MatriculaDTO
            {
                Id = m.Id,
                CursoId = m.CursoId,
                DataMatricula = m.DataMatricula,
                DataValidade = m.DataValidade,
                DataConclusao = m.DataConclusao,
                Status = m.Status.ToString()
            })
            .ToList();
    }

    public async Task<IEnumerable<CertificadoDTO>> Handle(ObterCertificadosAlunoQuery request, CancellationToken cancellationToken)
    {
        var aluno = await _alunoRepository.ObterAlunoComCertificados(request.AlunoId);

        if (aluno is null)
        {
            return Enumerable.Empty<CertificadoDTO>();
        }

        return aluno.Certificados
            .OrderByDescending(c => c.DataEmissao)
            .Select(c => new CertificadoDTO
            {
                Id = c.Id,
                CursoId = c.CursoId,
                DataEmissao = c.DataEmissao,
                Codigo = c.Codigo
            })
            .ToList();
    }

    public async Task<ProgressoCursoDTO?> Handle(ObterProgressoCursoQuery request, CancellationToken cancellationToken)
    {
        var aluno = await _alunoRepository.ObterAlunoComMatriculas(request.AlunoId);

        if (aluno is null)
        {
            return null;
        }

        var matricula = aluno.Matriculas.FirstOrDefault(m => m.CursoId == request.CursoId);

        if (matricula is null)
        {
            return null;
        }

        var curso = await _cursoRepository.ObterCursoComAulas(request.CursoId);

        var totalAulas = curso?.Aulas?.Count ?? 0;
        var aulasConcluidas = aluno.HistoricoAprendizado.ObterTotalAulasConcluidasPorCurso(request.CursoId);
        var percentual = totalAulas == 0
            ? 0
            : Math.Round((decimal)aulasConcluidas / totalAulas * 100, 2);

        return new ProgressoCursoDTO
        {
            CursoId = request.CursoId,
            TotalAulas = totalAulas,
            AulasConcluidas = aulasConcluidas,
            PercentualConcluido = percentual,
            StatusMatricula = matricula.Status.ToString()
        };
    }
}

