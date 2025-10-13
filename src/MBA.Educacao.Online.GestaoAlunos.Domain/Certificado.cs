using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.GestaoAlunos.Domain;

public class Certificado : Entity
{
    protected Certificado() { }

    public Certificado(Guid alunoId, Guid cursoId, DateTime dataEmissao, string codigo)
    {
        Validacoes.ValidarSeVazio(codigo, "O código do certificado não pode ser vazio");
        
        AlunoId = alunoId;
        CursoId = cursoId;
        DataEmissao = dataEmissao;
        Codigo = codigo;
        Valido = true;
    }

    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }
    public DateTime DataEmissao { get; private set; }
    public string Codigo { get; private set; }
    public bool Valido { get; private set; }
    public virtual Aluno Aluno { get; private set; }

    public void Invalidar() => Valido = false;

    public void Revalidar() => Valido = true;
}