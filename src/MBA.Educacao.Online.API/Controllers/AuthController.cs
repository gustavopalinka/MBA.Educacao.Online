using MBA.Educacao.Online.Core.Extensions;
using MBA.Educacao.Online.Core.Models;
using MBA.Educacao.Online.GestaoAlunos.Data;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using MBA.Educacao.Online.Security.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MBA.Educacao.Online.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SecurityDbContext _securityContext;
    private readonly AlunoContext _alunoContext;
    private readonly JwtSettings _jwtSettings;

    public AuthController(SignInManager<IdentityUser> signInManager,
                          UserManager<IdentityUser> userManager,
                          SecurityDbContext securityContext,
                          AlunoContext alunoContext,
                          IOptions<JwtSettings> jwtSettings)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _securityContext = securityContext;
        _alunoContext = alunoContext;
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> Register(RegisterUserViewModel registerUser)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var user = new IdentityUser(registerUser.UserName)
        {
            Email = registerUser.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, registerUser.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Aluno");

            var aluno = new Aluno(user.Id.ToGuid(), registerUser.Name, user.Email);
            _alunoContext.Alunos.Add(aluno);
            await _alunoContext.SaveChangesAsync();

            await _signInManager.SignInAsync(user, false);
            return Ok(await GerarJwt(registerUser.Email));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Login(LoginUserViewModel loginUser)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var user = await _userManager.FindByEmailAsync(loginUser.Email);
        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                return Ok(await GerarJwt(loginUser.Email));
            }
        }

        return Problem("Usuário ou senha incorretos", statusCode: 400);
    }

    private async Task<string> GerarJwt(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var roles = await _userManager.GetRolesAsync(user!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user!.Id),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Segredo);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtSettings.Emissor,
            Audience = _jwtSettings.Audiencia,
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        return tokenHandler.WriteToken(token);
    }
}