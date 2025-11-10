using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MBA.Educacao.Online.GestaoAlunos.Data;
using MBA.Educacao.Online.GestaoAlunos.Data.Repositories;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.GestaoAlunos;

public class AlunoRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AlunoContext _context;
    private readonly AlunoRepository _repository;

    public AlunoRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AlunoContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AlunoContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _repository = new AlunoRepository(_context);
    }

    [Trait("Categoria", "GestaoAlunos - Repositorio")]
    [Fact(DisplayName = "Deve adicionar aluno e buscar por Id")]
    public async Task Deve_Adicionar_Aluno_E_Buscar_Por_Id()
    {
        var aluno = CriarAluno();

        _repository.Adicionar(aluno);
        await _repository.UnitOfWork.Commit();

        var recuperado = await _repository.ObterPorId(aluno.Id);

        recuperado.Should().NotBeNull();
        recuperado!.Email.Should().Be(aluno.Email);
    }

    [Trait("Categoria", "GestaoAlunos - Repositorio")]
    [Fact(DisplayName = "Deve atualizar aluno existente")]
    public async Task Deve_Atualizar_Aluno()
    {
        var aluno = CriarAluno();

        _repository.Adicionar(aluno);
        await _repository.UnitOfWork.Commit();

        aluno.AtualizarInformacoes("Aluno Atualizado", "novo@teste.com");
        _repository.Atualizar(aluno);
        await _repository.UnitOfWork.Commit();

        var recuperado = await _repository.ObterPorId(aluno.Id);

        recuperado.Should().NotBeNull();
        recuperado!.Nome.Should().Be("Aluno Atualizado");
        recuperado.Email.Should().Be("novo@teste.com");
    }

    [Trait("Categoria", "GestaoAlunos - Repositorio")]
    [Fact(DisplayName = "Deve obter aluno com matrículas")]
    public async Task Deve_Obter_Aluno_Com_Matriculas()
    {
        var aluno = CriarAluno();
        var cursoId = Guid.NewGuid();
        aluno.MatricularEmCurso(cursoId);

        _repository.Adicionar(aluno);
        await _repository.UnitOfWork.Commit();

        var recuperado = await _repository.ObterAlunoComMatriculas(aluno.Id);

        recuperado.Should().NotBeNull();
        recuperado!.Matriculas.Should().ContainSingle(m => m.CursoId == cursoId);
    }

    [Trait("Categoria", "GestaoAlunos - Repositorio")]
    [Fact(DisplayName = "Deve obter aluno com certificados")]
    public async Task Deve_Obter_Aluno_Com_Certificados()
    {
        var aluno = CriarAluno();
        var cursoId = Guid.NewGuid();
        aluno.MatricularEmCurso(cursoId);
        var matricula = aluno.Matriculas.First();
        matricula.Ativar();
        aluno.ConcluirCurso(cursoId, matricula.Id);

        _repository.Adicionar(aluno);
        await _repository.UnitOfWork.Commit();

        var recuperado = await _repository.ObterAlunoComCertificados(aluno.Id);

        recuperado.Should().NotBeNull();
        recuperado!.Certificados.Should().ContainSingle(c => c.CursoId == cursoId);
    }

    [Trait("Categoria", "GestaoAlunos - Repositorio")]
    [Fact(DisplayName = "Deve obter aluno por email")]
    public async Task Deve_Obter_Por_Email()
    {
        var aluno = CriarAluno();

        _repository.Adicionar(aluno);
        await _repository.UnitOfWork.Commit();

        var recuperado = await _repository.ObterPorEmail(aluno.Email);

        recuperado.Should().NotBeNull();
        recuperado!.Id.Should().Be(aluno.Id);
    }

    [Trait("Categoria", "GestaoAlunos - Repositorio")]
    [Fact(DisplayName = "Deve obter todos os alunos")]
    public async Task Deve_Obter_Todos()
    {
        _repository.Adicionar(CriarAluno());
        _repository.Adicionar(CriarAluno());
        await _repository.UnitOfWork.Commit();

        var alunos = await _repository.ObterTodos();

        alunos.Should().HaveCount(2);
    }

    private static Aluno CriarAluno()
    {
        return new Aluno(Guid.NewGuid(), $"Aluno {Guid.NewGuid():N}", $"{Guid.NewGuid():N}@teste.com");
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}


