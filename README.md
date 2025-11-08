# MBA Projeto 03 Plataforma de Educação Online

# CREDENCIAIS DE TESTE:

## Admin:
Email: admin@mba03.com
Senha: Admin@123

## Aluno:
Email: aluno@teste.com
Senha: Aluno@123

## 📁 Estrutura do Projeto

```
MBA.Educacao.Online/
├── src/
│   ├── Services/
│   │   ├── 1-Core/
│   │   │   └── MBA.Educacao.Online.Core
│   │   ├── 2-BoundedContexts/
│   │   │   ├── GestaoConteudo/
│   │   │   │   ├── Domain
│   │   │   │   ├── Data
│   │   │   │   └── Application
│   │   │   ├── GestaoAlunos/
│   │   │   │   ├── Domain
│   │   │   │   ├── Data
│   │   │   │   └── Application
│   │   │   └── Pagamentos/
│   │   │       ├── Domain
│   │   │       ├── Data
│   │   │       └── Application
│   │   └── 3-CrossCutting/
│   │       ├── Security
│   │       └── Ioc
│   └── WebApps/
│       └── MBA.Educacao.Online.API
├── tests/
│   ├── UnitTests
│   └── IntegrationTests
└── docs/
```

## 🚀 Visão Geral

A plataforma expõe uma API única que integra os bounded contexts de **Gestão de Conteúdo**, **Gestão de Alunos** e **Pagamentos**.
Os principais casos de uso contemplados são:

- Cadastro e configuração de cursos e aulas pelo administrador.
- Matrícula de alunos autenticados e gestão de progresso por aula.
- Processamento de pagamentos com emissão de eventos que atualizam o status da matrícula.
- Finalização de cursos com geração automática de certificados.

O projeto utiliza **ASP.NET Core 8**, **MediatR** para orquestração CQRS, autenticação **JWT + Identity** e suporte a SQLite/SQL Server com seeding automático.

## 📦 Requisitos

- .NET SDK 8.0+
- SQL Server local ou Docker (opcional – o projeto sobe com SQLite seeded por padrão)
- Git

## 🔧 Configuração e Execução

1. Clonar o repositório e restaurar dependências:
   ```bash
   git clone <repo>
   cd MBA.Educacao.Online
   dotnet restore
   ```

2. Rodar a API (usa SQLite + seeding por padrão):
   ```bash
   dotnet run --project src/MBA.Educacao.Online.API/MBA.Educacao.Online.API.csproj
   ```

3. A documentação Swagger fica disponível em `https://localhost:<porta>/swagger`.

4. Credenciais padrão (seed):
   - Admin: `admin@mba03.com` / `Admin@123`
   - Aluno: `aluno@teste.com` / `Aluno@123`

## ✅ Testes Automatizados

Executar toda a suíte (unitários + integração):
```bash
dotnet test
```

- **UnitTests** validam regras de domínio e handlers (ex.: matrícula, pagamentos).
- **IntegrationTests** percorrem o fluxo fim-a-fim (matrícula → pagamento → progresso → finalização) usando repositórios in-memory.

## 🔐 Autenticação

- `POST /api/Auth/login` – retorna JWT.
- `POST /api/Auth/register` – cria novo aluno + usuário Identity.
- Os endpoints exigem tokens Bearer e perfis (`Administrador` ou `Aluno`).

## 📚 Casos de Uso

- `POST /api/Curso` (admin) – cria curso.
- `POST /api/Curso/{cursoId}/aulas` (admin) – adiciona aula.
- `POST /api/Matricula` (aluno) – matricula em curso.
- `POST /api/Matricula/progresso` (aluno) – registra aula concluída.
- `POST /api/Matricula/finalizar` (aluno) – gera certificado.
- `POST /api/Pagamento` (aluno) – processa pagamento.
- `POST /api/Pagamento/{id}/confirmar` (admin) – confirma pagamento manualmente.

## 🧱 Arquitetura

- Cada bounded context possui camadas **Domain**, **Data** e **Application**.
- Eventos de pagamento (`PagamentoConfirmadoEvent` / `PagamentoRejeitadoEvent`) atualizam automaticamente o status da matrícula via handlers.
- IoC centralizado em `MBA.Educacao.Online.Ioc`.
- Seeds automáticos para dados básicos e usuários padrão (`DatabaseConfig.EnsureDatabaseCreated`).

## 📄 Documentação Complementar

- `FEEDBACK.md` – consolidará os feedbacks das entregas.
- `docs/` – espaço reservado para artefatos auxiliares (diagramas, notas, etc.).

---

> Dúvidas, sugestões ou melhorias? Abra uma issue ou PR! 😊