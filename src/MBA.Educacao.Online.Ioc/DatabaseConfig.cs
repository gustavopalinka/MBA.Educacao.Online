using MBA.Educacao.Online.GestaoAlunos.Data;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using MBA.Educacao.Online.GestaoConteudo.Data;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using MBA.Educacao.Online.Pagamentos.Data;
using MBA.Educacao.Online.Security.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MBA.Educacao.Online.Ioc;

public static class DatabaseConfig
{
    public static void EnsureDatabaseCreated(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var services = serviceScope.ServiceProvider;

        var securityContext = services.GetRequiredService<SecurityDbContext>();
        var conteudoContext = services.GetRequiredService<ConteudoContext>();
        var alunoContext = services.GetRequiredService<AlunoContext>();
        var pagamentoContext = services.GetRequiredService<PagamentoContext>();

        securityContext.Database.Migrate();
        conteudoContext.Database.Migrate();
        alunoContext.Database.Migrate();
        pagamentoContext.Database.Migrate();

        SeedData(services, securityContext, conteudoContext, alunoContext).Wait();
    }

    private static async Task SeedData(IServiceProvider services, SecurityDbContext securityContext, 
                                       ConteudoContext conteudoContext, AlunoContext alunoContext)
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync("Administrador"))
        {
            await roleManager.CreateAsync(new IdentityRole("Administrador"));
        }

        if (!await roleManager.RoleExistsAsync("Aluno"))
        {
            await roleManager.CreateAsync(new IdentityRole("Aluno"));
        }

        var adminEmail = "admin@mba03.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new IdentityUser
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Administrador");
            }
        }

        var alunoEmail = "aluno@teste.com";
        if (await userManager.FindByEmailAsync(alunoEmail) == null)
        {
            var alunoUser = new IdentityUser
            {
                UserName = "aluno.teste",
                Email = alunoEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(alunoUser, "Aluno@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(alunoUser, "Aluno");

                var aluno = new Aluno(Guid.Parse(alunoUser.Id), "Aluno Teste", alunoEmail);
                alunoContext.Alunos.Add(aluno);
                await alunoContext.SaveChangesAsync();
            }
        }

        if (!conteudoContext.Cursos.Any())
        {
            var conteudoProgramatico1 = new ConteudoProgramatico(
                "Introdução ao C#, POO, LINQ, Async/Await, Entity Framework", 
                1, 
                DateTime.Now
            );

            var curso1 = new Curso(
                "Fundamentos de C# e .NET",
                "Aprenda os fundamentos da linguagem C# e do ecossistema .NET",
                199.90m,
                40,
                "Iniciantes em programação",
                "Dominar os conceitos básicos de C# e .NET",
                "Conhecimento básico de lógica de programação",
                conteudoProgramatico1
            );

            var aula1 = new Aula("AULA01", "Introdução ao C#", "Primeiros passos com C#", 1, curso1.Id);
            var aula2 = new Aula("AULA02", "Orientação a Objetos", "POO em C#", 2, curso1.Id);
            curso1.AdicionarAula(aula1);
            curso1.AdicionarAula(aula2);

            conteudoContext.Cursos.Add(curso1);

            var conteudoProgramatico2 = new ConteudoProgramatico(
                "DDD, Clean Architecture, CQRS, Event Sourcing, Microservices", 
                1, 
                DateTime.Now
            );

            var curso2 = new Curso(
                "Arquitetura de Software Avançada",
                "Aprenda padrões arquiteturais modernos",
                299.90m,
                60,
                "Desenvolvedores intermediários/avançados",
                "Dominar arquitetura de software enterprise",
                "Experiência com C# e desenvolvimento web",
                conteudoProgramatico2
            );

            var aula3 = new Aula("AULA01", "Domain-Driven Design", "Conceitos de DDD", 1, curso2.Id);
            var aula4 = new Aula("AULA02", "Clean Architecture", "Camadas e responsabilidades", 2, curso2.Id);
            curso2.AdicionarAula(aula3);
            curso2.AdicionarAula(aula4);

            conteudoContext.Cursos.Add(curso2);

            await conteudoContext.SaveChangesAsync();
        }
    }
}