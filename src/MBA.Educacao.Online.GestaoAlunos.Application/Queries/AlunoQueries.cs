using System;
using System.Collections.Generic;
using MBA.Educacao.Online.GestaoAlunos.Application.DTOs;
using MediatR;

namespace MBA.Educacao.Online.GestaoAlunos.Application.Queries;

public record ObterMatriculasAlunoQuery(Guid AlunoId) : IRequest<IEnumerable<MatriculaDTO>>;

public record ObterCertificadosAlunoQuery(Guid AlunoId) : IRequest<IEnumerable<CertificadoDTO>>;

public record ObterProgressoCursoQuery(Guid AlunoId, Guid CursoId) : IRequest<ProgressoCursoDTO?>;

