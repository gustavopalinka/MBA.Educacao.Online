using MBA.Educacao.Online.Core.Mediator;
using MBA.Educacao.Online.GestaoAlunos.Application.Commands;
using MBA.Educacao.Online.GestaoAlunos.Data;
using MBA.Educacao.Online.GestaoAlunos.Data.Repositories;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using MBA.Educacao.Online.GestaoConteudo.Application.Commands;
using MBA.Educacao.Online.GestaoConteudo.Data;
using MBA.Educacao.Online.GestaoConteudo.Data.Repositories;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using MBA.Educacao.Online.Pagamentos.Application.Commands;
using MBA.Educacao.Online.Pagamentos.Data;
using MBA.Educacao.Online.Pagamentos.Data.Repositories;
using MBA.Educacao.Online.Pagamentos.Domain;
using MBA.Educacao.Online.Security.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MBA.Educacao.Online.Ioc;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContexts(services, configuration);
        AddRepositories(services);
        AddMediatR(services);
        AddMediatorHandler(services);
        return services;
    }

    private static void AddDbContexts(IServiceCollection services, IConfiguration configuration)
    {
        var useSqlite = !string.IsNullOrEmpty(configuration.GetConnectionString("DefaultConnectionSqlite"));

        if (useSqlite)
        {
            var sqliteConnection = configuration.GetConnectionString("DefaultConnectionSqlite");
            var migrationsAssembly = "MBA.Educacao.Online.API";

            services.AddDbContext<SecurityDbContext>(options =>
                options.UseSqlite(sqliteConnection, 
                    b => b.MigrationsAssembly(migrationsAssembly)));

            services.AddDbContext<ConteudoContext>(options =>
                options.UseSqlite(sqliteConnection,
                    b => b.MigrationsAssembly(migrationsAssembly)));

            services.AddDbContext<AlunoContext>(options =>
                options.UseSqlite(sqliteConnection,
                    b => b.MigrationsAssembly(migrationsAssembly)));

            services.AddDbContext<PagamentoContext>(options =>
                options.UseSqlite(sqliteConnection,
                    b => b.MigrationsAssembly(migrationsAssembly)));
        }
        else
        {
            var sqlServerConnection = configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = "MBA.Educacao.Online.API";

            services.AddDbContext<SecurityDbContext>(options =>
                options.UseSqlServer(sqlServerConnection,
                    b => b.MigrationsAssembly(migrationsAssembly)));

            services.AddDbContext<ConteudoContext>(options =>
                options.UseSqlServer(sqlServerConnection,
                    b => b.MigrationsAssembly(migrationsAssembly)));

            services.AddDbContext<AlunoContext>(options =>
                options.UseSqlServer(sqlServerConnection,
                    b => b.MigrationsAssembly(migrationsAssembly)));

            services.AddDbContext<PagamentoContext>(options =>
                options.UseSqlServer(sqlServerConnection,
                    b => b.MigrationsAssembly(migrationsAssembly)));
        }
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<ICursoRepository, CursoRepository>();
        services.AddScoped<IAlunoRepository, AlunoRepository>();
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
    }

    private static void AddMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MatricularAlunoCommand).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CriarCursoCommand).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RealizarPagamentoCommand).Assembly));
    }

    private static void AddMediatorHandler(IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<DomainNotificationHandler>();
        services.AddScoped<INotificationHandler<DomainNotification>>(sp => sp.GetRequiredService<DomainNotificationHandler>());
    }
}