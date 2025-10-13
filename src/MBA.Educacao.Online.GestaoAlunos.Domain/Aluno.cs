using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.GestaoAlunos.Domain;

/// <summary>
/// Aggregate Root: Aluno
/// Representa um aluno da plataforma
/// </summary>
public class Aluno : Entity, IAggregateRoot
{
    protected Aluno() 
    {
        Matriculas = new List<Matricula>();
        Certificados = new List<Certificado>();
    }

    public Aluno(Guid usuarioId, string nome, string email, HistoricoAprendizado? historicoAprendizado = null)
    {
        ValidarAluno(nome, email);
        
        Id = usuarioId; // Compartilha o mesmo ID do usuário (Identity)
        Nome = nome;
        Email = email;
        HistoricoAprendizado = historicoAprendizado ?? new HistoricoAprendizado();
        DataCadastro = DateTime.Now;
        Ativo = true;
        Matriculas = new List<Matricula>();
        Certificados = new List<Certificado>();
    }

    public string Nome { get; private set; }
    public string Email { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public bool Ativo { get; private set; }

    // Value Object
    public HistoricoAprendizado HistoricoAprendizado { get; private set; }

    // Navegação
    public ICollection<Matricula> Matriculas { get; private set; }
    public ICollection<Certificado> Certificados { get; private set; }

    // Comportamentos
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
        if (matricula != null)
        {
            matricula.Concluir();
            
            // Gerar certificado
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
        // Formato: CERT-{AlunoId-Curto}-{CursoId-Curto}-{Timestamp}
        var alunoShort = Id.ToString().Substring(0, 8);
        var cursoShort = cursoId.ToString().Substring(0, 8);
        var timestamp = DateTime.Now.ToString("yyyyMMdd");
        return $"CERT-{alunoShort}-{cursoShort}-{timestamp}";
    }

    // Validações
    private void ValidarAluno(string nome, string email)
    {
        Validacoes.ValidarSeVazio(nome, "O nome do aluno não pode ser vazio");
        Validacoes.ValidarTamanho(nome, NomeMaxLength, $"O nome do aluno deve ter no máximo {NomeMaxLength} caracteres");
        
        Validacoes.ValidarSeVazio(email, "O email do aluno não pode ser vazio");
        Validacoes.ValidarTamanho(email, EmailMaxLength, $"O email do aluno deve ter no máximo {EmailMaxLength} caracteres");
    }

    public override bool EhValido()
    {
        return !string.IsNullOrEmpty(Nome) && !string.IsNullOrEmpty(Email);
    }

    #region Constants
    public const int NomeMaxLength = 200;
    public const int EmailMaxLength = 200;
    #endregion
}

