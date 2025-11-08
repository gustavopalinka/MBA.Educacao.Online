using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MBA.Educacao.Online.Core.Data;
using MBA.Educacao.Online.GestaoAlunos.Application.Commands;
using MBA.Educacao.Online.GestaoAlunos.Application.EventHandlers;
using MBA.Educacao.Online.GestaoAlunos.Application.Handlers;
using MBA.Educacao.Online.GestaoAlunos.Application.Queries;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using MBA.Educacao.Online.GestaoConteudo.Application.Commands;
using MBA.Educacao.Online.GestaoConteudo.Application.Handlers;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using MBA.Educacao.Online.Pagamentos.Application.Commands;
using MBA.Educacao.Online.Pagamentos.Application.Handlers;
using MBA.Educacao.Online.Pagamentos.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MBA.Educacao.Online.IntegrationTests;

public class FluxoCompletoTests : IAsyncLifetime
{
    private readonly ServiceProvider _provider;

    public FluxoCompletoTests()
    {
        var services = new ServiceCollection();

        services.AddSingleton<FakeUnitOfWork>();
        services.AddSingleton<IAlunoRepository, FakeAlunoRepository>();
        services.AddSingleton<ICursoRepository, FakeCursoRepository>();
        services.AddSingleton<IPagamentoRepository, FakePagamentoRepository>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CriarCursoCommand).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(AlunoCommandHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(PagamentoCommandHandler).Assembly);
        });

        _provider = services.BuildServiceProvider();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        _provider.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Deve_Executar_Fluxo_Completo_Com_Sucesso()
    {
        using var scope = _provider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var cursoRepository = scope.ServiceProvider.GetRequiredService<ICursoRepository>();
        var alunoRepository = scope.ServiceProvider.GetRequiredService<IAlunoRepository>();

        var conteudoProgramatico = new ConteudoProgramatico("DDD, CQRS, Clean Architecture", 1, DateTime.UtcNow);
        var curso = new Curso("Arquitetura Avançada", "Curso completo", 300m, 60,
            "Desenvolvedores", "Dominar arquitetura enterprise", "Conhecimento em C#", conteudoProgramatico);

        curso.AdicionarAula(new Aula("AULA01", "Domain-Driven Design", "Fundamentos de DDD", 1, curso.Id));
        curso.AdicionarAula(new Aula("AULA02", "CQRS", "Separação de comandos e queries", 2, curso.Id));

        cursoRepository.Adicionar(curso);
        await cursoRepository.UnitOfWork.Commit();

        var aluno = new Aluno(Guid.NewGuid(), "Aluno Integração", "integracao@teste.com");
        alunoRepository.Adicionar(aluno);
        await alunoRepository.UnitOfWork.Commit();

        var matricular = await mediator.Send(new MatricularAlunoCommand(aluno.Id, curso.Id));
        matricular.Should().BeTrue();

        var alunoMatriculado = await alunoRepository.ObterAlunoComMatriculas(aluno.Id);
        alunoMatriculado.Should().NotBeNull();
        var matricula = alunoMatriculado!.Matriculas.First();

        var pagamento = await mediator.Send(new RealizarPagamentoCommand(matricula.Id, aluno.Id, curso.Valor,
            "5555444433332222", "Aluno Integração", "12/30", "123"));
        pagamento.Should().BeTrue();

        alunoMatriculado = await alunoRepository.ObterAlunoComMatriculas(aluno.Id);
        matricula = alunoMatriculado!.Matriculas.First();
        matricula.Status.Should().Be(StatusMatricula.Ativa);

        var cursoComAulas = await cursoRepository.ObterCursoComAulas(curso.Id);
        cursoComAulas.Should().NotBeNull();

        foreach (var aula in cursoComAulas!.Aulas)
        {
            var progresso = await mediator.Send(new RegistrarProgressoCommand(aluno.Id, curso.Id, aula.Id));
            progresso.Should().BeTrue();
        }

        var finalizar = await mediator.Send(new FinalizarCursoCommand(aluno.Id, curso.Id, matricula.Id));
        finalizar.Should().BeTrue();

        var certificados = await mediator.Send(new ObterCertificadosAlunoQuery(aluno.Id));
        certificados.Should().ContainSingle();

        var progressoCurso = await mediator.Send(new ObterProgressoCursoQuery(aluno.Id, curso.Id));
        progressoCurso.Should().NotBeNull();
        progressoCurso!.PercentualConcluido.Should().Be(100m);
    }

    private class FakeUnitOfWork : IUnitOfWork
    {
        public Task<bool> Commit() => Task.FromResult(true);
    }

    private class FakeAlunoRepository : IAlunoRepository
    {
        private readonly ConcurrentDictionary<Guid, Aluno> _alunos = new();
        private readonly FakeUnitOfWork _unitOfWork;

        public FakeAlunoRepository(FakeUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork => _unitOfWork;

        public void Adicionar(Aluno aluno) => _alunos[aluno.Id] = aluno;

        public Task<Aluno?> ObterPorId(Guid id) =>
            Task.FromResult(_alunos.TryGetValue(id, out var aluno) ? aluno : null);

        public Task<Aluno?> ObterPorEmail(string email) =>
            Task.FromResult(_alunos.Values.FirstOrDefault(a => a.Email == email));

        public Task<Aluno?> ObterAlunoComMatriculas(Guid id) => ObterPorId(id);

        public Task<Aluno?> ObterAlunoComCertificados(Guid id) => ObterPorId(id);

        public Task<IEnumerable<Aluno>> ObterTodos() =>
            Task.FromResult(_alunos.Values.AsEnumerable());

        public void Atualizar(Aluno aluno) => _alunos[aluno.Id] = aluno;

        public void Dispose() { }
    }

    private class FakeCursoRepository : ICursoRepository
    {
        private readonly ConcurrentDictionary<Guid, Curso> _cursos = new();
        private readonly FakeUnitOfWork _unitOfWork;

        public FakeCursoRepository(FakeUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork => _unitOfWork;

        public void Adicionar(Curso curso) => _cursos[curso.Id] = curso;

        public void Atualizar(Curso curso) => _cursos[curso.Id] = curso;

        public void Remover(Curso curso) => _cursos.TryRemove(curso.Id, out _);

        public Task<Curso?> ObterPorId(Guid id) =>
            Task.FromResult(_cursos.TryGetValue(id, out var curso) ? curso : null);

        public Task<IEnumerable<Curso>> ObterTodos() =>
            Task.FromResult(_cursos.Values.AsEnumerable());

        public Task<IEnumerable<Curso>> ObterCursosAtivos() =>
            Task.FromResult(_cursos.Values.Where(c => c.Ativo));

        public Task<Curso?> ObterCursoComAulas(Guid id) => ObterPorId(id);

        public void Dispose() { }
    }

    private class FakePagamentoRepository : IPagamentoRepository
    {
        private readonly ConcurrentDictionary<Guid, Pagamento> _pagamentos = new();
        private readonly FakeUnitOfWork _unitOfWork;

        public FakePagamentoRepository(FakeUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork => _unitOfWork;

        public void Adicionar(Pagamento pagamento) => _pagamentos[pagamento.Id] = pagamento;

        public void Atualizar(Pagamento pagamento) => _pagamentos[pagamento.Id] = pagamento;

        public Task<Pagamento?> ObterPorId(Guid id) =>
            Task.FromResult(_pagamentos.TryGetValue(id, out var pagamento) ? pagamento : null);

        public Task<Pagamento?> ObterPorMatricula(Guid matriculaId) =>
            Task.FromResult(_pagamentos.Values.FirstOrDefault(p => p.MatriculaId == matriculaId));

        public Task<IEnumerable<Pagamento>> ObterPorAluno(Guid alunoId) =>
            Task.FromResult(_pagamentos.Values.Where(p => p.AlunoId == alunoId));

        public void Dispose() { }
    }
}

