namespace MBA.Educacao.Online.Core.Models;

/// <summary>
/// Configurações do JWT para autenticação.
/// </summary>
public class JwtSettings
{
    public string Segredo { get; set; } = string.Empty;
    public int ExpiracaoHoras { get; set; }
    public string Emissor { get; set; } = string.Empty;
    public string Audiencia { get; set; } = string.Empty;
}

