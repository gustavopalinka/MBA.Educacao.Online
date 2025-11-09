using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.GestaoAlunos.Domain;

public class Aluno : Entity, IAggregateRoot
{
    protected Aluno() { }

    public Aluno(Guid usuarioId, string nome, string email, HistoricoAprendizado? historicoAprendizado = null)
    {
        ValidarAluno(nome, email);
        
        Id = usuarioId;
        Nome = nome;
        Email = email;
        HistoricoAprendizado = historicoAprendizado ?? new HistoricoAprendizado();
        DataCadastro = DateTime.Now;
        Ativo = true;
        Matriculas = new List<Matricula>();
        Certificados = new List<Certificado>();
    }

    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime DataCadastro { get; private set; }
    public bool Ativo { get; private set; }
    public HistoricoAprendizado HistoricoAprendizado { get; private set; } = new();
    public ICollection<Matricula> Matriculas { get; private set; } = new List<Matricula>();
    public ICollection<Certificado> Certificados { get; private set; } = new List<Certificado>();

    public void AlterarStatus(bool ativo) => Ativo = ativo;

    public void AtualizarInformacoes(string nome, string email)
    {
        ValidarAluno(nome, email);
        Nome = nome;
        Email = email;
    }

    public void MatricularEmCurso(Guid cursoId)
    {
        var matricula = new Matricula(Id, cursoId, DateTime.Now);
        Matriculas.Add(matricula);
    }

    public void ConcluirCurso(Guid cursoId, Guid matriculaId)
    {
        var matricula = Matriculas.FirstOrDefault(m => m.Id == matriculaId);
        if (matricula is not null)
        {
            matricula.Concluir();
            var codigoCertificado = GerarCodigoCertificado(cursoId);
            var certificado = new Certificado(Id, cursoId, DateTime.Now, codigoCertificado);
            Certificados.Add(certificado);
        }
    }

    public void RegistrarProgresso(Guid cursoId, Guid aulaId)
    {
        HistoricoAprendizado.RegistrarAulaConcluida(cursoId, aulaId);
    }

    private string GerarCodigoCertificado(Guid cursoId)
    {
        var alunoShort = Id.ToString().Substring(0, 8);
        var cursoShort = cursoId.ToString().Substring(0, 8);
        var timestamp = DateTime.Now.ToString("yyyyMMdd");
        return $"CERT-{alunoShort}-{cursoShort}-{timestamp}";
    }

    private void ValidarAluno(string nome, string email)
    {
        Validacoes.ValidarSeVazio(nome, "O nome do aluno não pode ser vazio");
        Validacoes.ValidarSeVazio(email, "O email do aluno não pode ser vazio");
    }

    public override bool EhValido()
    {
        return !string.IsNullOrEmpty(Nome) && !string.IsNullOrEmpty(Email);
    }
}