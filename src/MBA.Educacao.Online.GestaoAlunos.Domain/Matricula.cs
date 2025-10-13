using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.GestaoAlunos.Domain;

public class Matricula : Entity
{
    protected Matricula() { }

    public Matricula(Guid alunoId, Guid cursoId, DateTime dataMatricula)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
        DataMatricula = dataMatricula;
        DataValidade = dataMatricula.AddYears(2);
        Status = StatusMatricula.Pendente;
        Ativo = true;
    }

    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }
    public DateTime DataMatricula { get; private set; }
    public DateTime DataValidade { get; private set; }
    public DateTime? DataConclusao { get; private set; }
    public StatusMatricula Status { get; private set; }
    public bool Ativo { get; private set; }
    public virtual Aluno Aluno { get; private set; }

    public void AlterarStatus(bool ativo) => Ativo = ativo;

    public void Ativar()
    {
        Status = StatusMatricula.Ativa;
    }

    public void Concluir()
    {
        Status = StatusMatricula.Concluida;
        DataConclusao = DateTime.Now;
    }

    public void Cancelar()
    {
        Status = StatusMatricula.Cancelada;
        Ativo = false;
    }

    public bool EstaVencida()
    {
        return DateTime.Now > DataValidade && Status != StatusMatricula.Concluida;
    }
}

public enum StatusMatricula
{
    Pendente = 0,
    Ativa = 1,
    Concluida = 2,
    Cancelada = 3
}