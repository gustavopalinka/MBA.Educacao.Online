using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MBA.Educacao.Online.Pagamentos.Data;
using MBA.Educacao.Online.Pagamentos.Data.Repositories;
using MBA.Educacao.Online.Pagamentos.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.Pagamentos;

public class PagamentoRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly PagamentoContext _context;
    private readonly PagamentoRepository _repository;

    public PagamentoRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<PagamentoContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new PagamentoContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _repository = new PagamentoRepository(_context);
    }

    [Trait("Categoria", "Pagamentos - Repositorio")]
    [Fact(DisplayName = "Deve adicionar e obter pagamento por Id")]
    public async Task Deve_Adicionar_E_Obter_Pagamento_Por_Id()
    {
        var pagamento = CriarPagamento();

        _repository.Adicionar(pagamento);
        await _repository.UnitOfWork.Commit();

        var recuperado = await _repository.ObterPorId(pagamento.Id);

        recuperado.Should().NotBeNull();
        recuperado!.AlunoId.Should().Be(pagamento.AlunoId);
    }

    [Trait("Categoria", "Pagamentos - Repositorio")]
    [Fact(DisplayName = "Deve atualizar pagamento existente")]
    public async Task Deve_Atualizar_Pagamento()
    {
        var pagamento = CriarPagamento();

        _repository.Adicionar(pagamento);
        await _repository.UnitOfWork.Commit();

        pagamento.Confirmar();
        _repository.Atualizar(pagamento);
        await _repository.UnitOfWork.Commit();

        var recuperado = await _repository.ObterPorId(pagamento.Id);

        recuperado!.StatusPagamento.Status.Should().Be(StatusPagamentoEnum.Confirmado);
    }

    [Trait("Categoria", "Pagamentos - Repositorio")]
    [Fact(DisplayName = "Deve obter pagamento por matrícula")]
    public async Task Deve_Obter_Pagamento_Por_Matricula()
    {
        var pagamento = CriarPagamento();

        _repository.Adicionar(pagamento);
        await _repository.UnitOfWork.Commit();

        var recuperado = await _repository.ObterPorMatricula(pagamento.MatriculaId);

        recuperado.Should().NotBeNull();
        recuperado!.Id.Should().Be(pagamento.Id);
    }

    [Trait("Categoria", "Pagamentos - Repositorio")]
    [Fact(DisplayName = "Deve obter pagamentos por aluno ordenados por data")]
    public async Task Deve_Obter_Pagamentos_Por_Aluno_Ordenado()
    {
        var pagamento1 = CriarPagamento();
        var pagamento2 = new Pagamento(Guid.NewGuid(), pagamento1.AlunoId, 200m,
            new DadosCartao("5555444433332210", "Aluno Teste", "12/30", "123"));

        _repository.Adicionar(pagamento1);
        _repository.Adicionar(pagamento2);
        await _repository.UnitOfWork.Commit();

        pagamento1.Confirmar();
        _repository.Atualizar(pagamento1);
        await _repository.UnitOfWork.Commit();

        var pagamentos = (await _repository.ObterPorAluno(pagamento1.AlunoId)).ToList();

        pagamentos.Should().HaveCount(2);
        pagamentos.First().DataPagamento.Should().BeOnOrAfter(pagamentos.Last().DataPagamento);
    }

    private static Pagamento CriarPagamento()
    {
        return new Pagamento(Guid.NewGuid(), Guid.NewGuid(), 150m,
            new DadosCartao("5555444433332222", "Aluno Teste", "12/30", "123"));
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}


