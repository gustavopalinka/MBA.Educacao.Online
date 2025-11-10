# **MBA Projeto 03 - Plataforma de Educação Online com CQRS**



## **1. Apresentação**



Bem-vindo ao repositório do projeto **MBA Projeto 03 - Plataforma de Educação Online com CQRS**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e corresponde ao módulo de **DDD, CQRS e Arquitetura Limpa aplicada em ASP.NET Core**.

O objetivo principal é disponibilizar uma plataforma de cursos online que permite cadastrar conteúdos, matricular alunos, processar pagamentos e acompanhar a jornada de aprendizado por meio de uma API RESTful com autenticação JWT.

[![Coverage](https://codecov.io/gh/gustavopalinka/MBA.Educacao.Online/branch/bkp/graph/badge.svg?token=daec4255-c339-427a-aa67-e87fd30f8ef9)](https://codecov.io/gh/gustavopalinka/MBA.Educacao.Online?branch=bkp)



### **Autor(es)**

- **Gustavo Palinka**



## **2. Proposta do Projeto**



O projeto consiste em:



- **Plataforma de Educação Online:** API única responsável por gerir cursos, matrículas, certificações e pagamentos.
- **Bounded Contexts independentes:** Conteúdo, Alunos e Pagamentos com aplicação de DDD, CQRS e MediatR.
- **Autenticação e Autorização:** ASP.NET Core Identity e JWT diferenciando administradores e alunos autenticados.
- **Eventos de Domínio:** Integração entre pagamento e atualização de matrícula via notificações mediadas.
- **Testes Automatizados:** Suíte de testes unitários e de integração com meta de 80% de cobertura.



## **3. Tecnologias Utilizadas**



- **Linguagem de Programação:** C#
- **Frameworks:**
  - ASP.NET Core Web API
  - MediatR
  - FluentValidation
  - Entity Framework Core
- **Banco de Dados:** SQL Server (produção) e SQLite (ambiente de validação/seed)
- **Autenticação e Autorização:**
  - ASP.NET Core Identity
  - JWT (JSON Web Token)
- **Documentação da API:** Swagger / Swashbuckle
- **Cobertura de Código:** Coverlet + ReportGenerator + Codecov



## **4. Estrutura do Projeto**



A estrutura do projeto é organizada da seguinte forma:



- src/
  - MBA.Educacao.Online.API/ - Projeto Web API público
  - MBA.Educacao.Online.Core/ - Abstrações, eventos e base de domínio
  - MBA.Educacao.Online.GestaoConteudo.*/ - Bounded Context de cursos e aulas
  - MBA.Educacao.Online.GestaoAlunos.*/ - Bounded Context de alunos, matrículas e certificados
  - MBA.Educacao.Online.Pagamentos.*/ - Bounded Context de pagamentos e notificações
  - MBA.Educacao.Online.Ioc/ - Registro das dependências
  - MBA.Educacao.Online.Security/ - Configurações de Identity e JWT
- tests/
  - UnitTests/ - Testes unitários de comandos, eventos e domínios
  - IntegrationTests/ - Testes de fluxo completo e integração entre bounded contexts
- README.md - Documento principal do projeto
- FEEDBACK.md - Registro dos feedbacks do instrutor
- .gitignore - Padrões de exclusão do Git



## **5. Funcionalidades Implementadas**



- **Cadastro e gestão de cursos e aulas:** Administradores podem criar cursos, definir conteúdo programático e adicionar aulas.
- **Matrículas de alunos:** Alunos autenticados se matriculam em cursos, iniciando no status pendente.
- **Processamento de pagamentos:** Confirmação ou rejeição de pagamentos atualiza o status da matrícula via eventos de domínio.
- **Progresso e certificação:** Registro das aulas concluídas, cálculo do percentual e emissão de certificado ao final do curso.
- **API RESTful documentada:** Endpoints expostos com Swagger e protegidos por JWT.
- **Cobertura de testes:** Testes unitários e de integração validando casos de uso críticos.



## **6. Como Executar o Projeto**



### **Pré-requisitos**



- .NET SDK 8.0 ou superior
- SQL Server LocalDB ou instância compatível
- Visual Studio 2022 / VS Code / Rider (ou IDE de preferência)
- Git



### **Passos para Execução**



1. **Clone o Repositório:**
   - `git clone https://github.com/gustavopalinka/MBA.Educacao.Online.git`
   - `cd MBA.Educacao.Online`

2. **Restaurar dependências e compilar:**
   - `dotnet restore`
   - `dotnet build`

3. **Configuração do Banco de Dados:**
   - Ajuste a string de conexão no `appsettings.Development.json` se necessário.
   - O projeto cria e popula o banco automaticamente com Seed (incluindo usuários e dados básicos).

4. **Executar a API:**
   - `dotnet run --project src/MBA.Educacao.Online.API`
   - Acesse a documentação em: http://localhost:5001/swagger

5. **Executar testes com cobertura (opcional):**
   - `dotnet test tests/UnitTests/MBA.Educacao.Online.UnitTests.csproj --settings coverlet.runsettings --collect:"XPlat Code Coverage" --results-directory TestResults/coverage`

6. **Credenciais de teste:**
   - Administrador: `admin@mba03.com` / `Admin@123`
   - Aluno: `aluno@teste.com` / `Aluno@123`



## **7. Instruções de Configuração**



- **JWT e Identity:** As configurações de chave, issuer e audience estão em `appsettings.json`. Ajuste conforme o ambiente.
- **Seed de banco:** O Seed inicial cria perfis de Administrador e Aluno com as credenciais acima, além de cursos e aulas de exemplo.
- **Variáveis de ambiente:** Utilize `dotnet user-secrets` ou variáveis do sistema para proteger segredos em produção.



## **8. Documentação da API**



A documentação da API está disponível através do Swagger. Após iniciar a API, acesse:



http://localhost:5001/swagger



## **9. Avaliação**



- Este projeto integra o MBA DevXpert e não aceita contribuições externas.
- Dúvidas ou feedbacks devem ser tratados via Issues ou diretamente com o instrutor.
- O arquivo `FEEDBACK.md` será atualizado exclusivamente pelo avaliador conforme os retornos da revisão.
