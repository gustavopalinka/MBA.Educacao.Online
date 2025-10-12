using System.ComponentModel.DataAnnotations;

namespace MBA.Educacao.Online.Core.Models;

/// <summary>
/// ViewModel para login de usuário.
/// </summary>
public class LoginUserViewModel
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public required string Password { get; set; }
}

