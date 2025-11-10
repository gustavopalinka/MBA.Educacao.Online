# MBA Projeto 03 Plataforma de Educação Online

[![Coverage](https://codecov.io/gh/gustavopalinka/MBA.Educacao.Online/branch/bkp/graph/badge.svg?token=daec4255-c339-427a-aa67-e87fd30f8ef9)](https://codecov.io/gh/gustavopalinka/MBA.Educacao.Online?branch=bkp)
[![Coverage](https://codecov.io/github/gustavopalinka/MBA.Educacao.Online/branch/main/graph/badge.svg)](https://codecov.io/github/gustavopalinka/MBA.Educacao.Online)

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
│   ├── MBA.Educacao.Online.Core
│   ├── MBA.Educacao.Online.GestaoConteudo.*
│   ├── MBA.Educacao.Online.GestaoAlunos.*
│   ├── MBA.Educacao.Online.Pagamentos.*
│   ├── MBA.Educacao.Online.Ioc
│   ├── MBA.Educacao.Online.Security
│   └── MBA.Educacao.Online.API
├── tests/
│   ├── UnitTests
│   └── IntegrationTests
```

### API – Controllers
- `AuthController` – registro/login com Identity + JWT.
- `CursoController` – comandos/queries para cursos e aulas.
- `AlunosController` – comandos (`POST matriculas`, `POST progresso`, `POST finalizar`) e consultas (`GET matriculas`, `GET {cursoId}/progresso`, `GET certificados`) para o aluno autenticado.
- `PagamentoController` – `POST` aluno para iniciar pagamento e `POST {id}/confirmar` para o administrador.

### CQRS + MediatR
- Todos os comandos de aluno/pagamento utilizam validators FluentValidation e são processados por handlers herdando de `CommandHandler`, que publica notificações de domínio (`DomainNotification`) via `IMediatorHandler`.
- Consultas (`AlunoQueryHandler`, `CursoQueryHandler`) retornam DTOs definidos em `Application/DTOs`.
- Eventos de pagamento (`PagamentoConfirmadoEvent`, `PagamentoRejeitadoEvent`) são tratados em `PagamentoEventHandler` para atualizar matrículas.

### Testes
- `tests/UnitTests` cobre handlers de GestaoAlunos e Pagamentos.
- `tests/IntegrationTests` exercita o fluxo completo (matrícula, pagamento, progresso, finalização e certificado) com dependências fake em memória.
- Rodar cobertura: `dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura`.
