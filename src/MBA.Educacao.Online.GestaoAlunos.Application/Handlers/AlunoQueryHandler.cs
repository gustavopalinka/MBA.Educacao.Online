using System;
using System.Collections.Generic;
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

        var resultado = new List<MatriculaDTO>();

        foreach (var matricula in aluno.Matriculas.OrderByDescending(m => m.DataMatricula))
        {
            var curso = await _cursoRepository.ObterPorId(matricula.CursoId);
            resultado.Add(new MatriculaDTO
            {
                Id = matricula.Id,
                CursoId = matricula.CursoId,
                NomeCurso = curso?.Nome ?? "Curso não encontrado",
                DataMatricula = matricula.DataMatricula,
                DataValidade = matricula.DataValidade,
                DataConclusao = matricula.DataConclusao,
                Status = matricula.Status.ToString()
            });
        }

        return resultado;
    }

    public async Task<IEnumerable<CertificadoDTO>> Handle(ObterCertificadosAlunoQuery request, CancellationToken cancellationToken)
    {
        var aluno = await _alunoRepository.ObterAlunoComCertificados(request.AlunoId);

        if (aluno is null)
        {
            return Enumerable.Empty<CertificadoDTO>();
        }

        var resultado = new List<CertificadoDTO>();

        foreach (var certificado in aluno.Certificados.OrderByDescending(c => c.DataEmissao))
        {
            var curso = await _cursoRepository.ObterPorId(certificado.CursoId);
            resultado.Add(new CertificadoDTO
            {
                Id = certificado.Id,
                CursoId = certificado.CursoId,
                NomeCurso = curso?.Nome ?? "Curso não encontrado",
                DataEmissao = certificado.DataEmissao,
                CodigoCertificado = certificado.Codigo
            });
        }

        return resultado;
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

        var aulas = curso?.Aulas?.Select(aula => new AulaProgressoDTO
        {
            AulaId = aula.Id,
            TituloAula = aula.Titulo,
            Concluida = aluno.HistoricoAprendizado.AulaJaConcluida(aula.Id),
            DataConclusao = aluno.HistoricoAprendizado.ObterDataConclusao(aula.Id)
        }).ToList() ?? new List<AulaProgressoDTO>();

        return new ProgressoCursoDTO
        {
            CursoId = request.CursoId,
            NomeCurso = curso?.Nome ?? "Curso não encontrado",
            TotalAulas = totalAulas,
            AulasConcluidas = aulasConcluidas,
            PercentualConcluido = percentual,
            StatusMatricula = matricula.Status.ToString(),
            Aulas = aulas
        };
    }
}

