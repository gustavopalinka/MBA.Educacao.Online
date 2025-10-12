# 📚 MBA.EducaOn - Plataforma de Educação Online

## 📋 Sobre o Projeto

Plataforma educacional online desenvolvida como projeto do MBA DevXpert, aplicando **Domain-Driven Design (DDD)**, **Test-Driven Development (TDD)**, **CQRS** e padrões arquiteturais modernos para gestão eficiente de conteúdos educacionais, alunos e processos financeiros.

---

## 🎯 Objetivos

Desenvolver uma plataforma com múltiplos **Bounded Contexts** para:
- ✅ Gestão de Cursos e Aulas
- ✅ Gestão de Alunos e Matrículas
- ✅ Processamento de Pagamentos
- ✅ Emissão de Certificados
- ✅ Controle de Progresso de Aprendizado

---

## 🏗️ Arquitetura

### **Bounded Contexts**

#### **BC1: Gestão de Conteúdo**
- **Aggregate Root:** Curso
- **Entities:** Aula
- **Value Objects:** ConteudoProgramatico

#### **BC2: Gestão de Alunos**
- **Aggregate Root:** Aluno
- **Entities:** Matricula, Certificado
- **Value Objects:** HistoricoAprendizado

#### **BC3: Pagamento e Faturamento**
- **Aggregate Root:** Pagamento
- **Value Objects:** DadosCartao, StatusPagamento

---

## 🛠️ Tecnologias Utilizadas

- **Linguagem:** C# (.NET 8.0)
- **Backend:** ASP.NET Core Web API
- **ORM:** Entity Framework Core
- **Banco de Dados:** SQL Server / SQLite
- **Autenticação:** JWT + ASP.NET Core Identity
- **Documentação:** Swagger/OpenAPI
- **Testes:** xUnit
- **Versionamento:** Git/GitHub

---

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

---

## 🚀 Como Executar

### **Pré-requisitos**

- .NET 8.0 SDK
- Visual Studio 2022 ou VS Code
- SQL Server (opcional, pode usar SQLite)

### **Passos**

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/seu-usuario/MBA.Educacao.Online.git
   cd MBA.Educacao.Online
   ```

2. **Restaure as dependências:**
   ```bash
   dotnet restore
   ```

3. **Configure o banco de dados:**
   - O projeto está configurado para usar **SQLite por padrão**
   - Para SQL Server, ajuste a connection string em `appsettings.json`

4. **Execute as migrations:**
   ```bash
   dotnet ef database update --project src/MBA.Educacao.Online.API
   ```

5. **Execute a aplicação:**
   ```bash
   dotnet run --project src/MBA.Educacao.Online.API
   ```

6. **Acesse o Swagger:**
   ```
   https://localhost:7000/swagger
   ```

---

## 🧪 Testes

### **Executar todos os testes:**
```bash
dotnet test
```

### **Executar testes com cobertura:**
```bash
dotnet test /p:CollectCoverage=true /p:CoverageReportFormat=opencover
```

**Meta de Cobertura:** 80% (conforme requisitos do projeto)

---

## 👥 Tipos de Usuário

### **Administrador**
- Cadastro e gestão de cursos e aulas
- Monitoramento de alunos
- Gestão financeira

### **Aluno**
- Matrícula em cursos
- Acesso a aulas e materiais
- Realização de pagamentos
- Acompanhamento de progresso
- Download de certificados

---

## 📚 Casos de Uso Principais

1. **Cadastro de Curso** (Admin)
2. **Cadastro de Aula** (Admin)
3. **Matrícula do Aluno** (Aluno)
4. **Realização de Pagamento** (Aluno)
5. **Acesso à Aula** (Aluno)
6. **Finalização do Curso** (Aluno)
7. **Emissão de Certificado** (Sistema)

---

## 📖 Documentação

- [Arquitetura](docs/arquitetura.md)
- [Casos de Uso Detalhados](docs/casos-de-uso.md)
- [Guia de Contribuição](docs/contribuicao.md)
- [API Reference](docs/api-reference.md)

---

## 🎓 Padrões e Práticas

- ✅ **Domain-Driven Design (DDD)**
- ✅ **Test-Driven Development (TDD)**
- ✅ **CQRS (Command Query Responsibility Segregation)**
- ✅ **Repository Pattern**
- ✅ **Unit of Work Pattern**
- ✅ **Dependency Injection**
- ✅ **Clean Architecture**
- ✅ **SOLID Principles**

---

## 📊 Status do Projeto

🚧 **Em Desenvolvimento**

- [x] Estrutura base do projeto
- [ ] Implementação BC1: Gestão de Conteúdo
- [ ] Implementação BC2: Gestão de Alunos
- [ ] Implementação BC3: Pagamentos
- [ ] Testes Unitários (80% cobertura)
- [ ] Testes de Integração
- [ ] Documentação completa

---

## 👨‍💻 Autor

**[Seu Nome]**
- GitHub: [@gustavopalinka](https://github.com/gustavopalinka)
- Email: gustavo.scabuzzi@gmail.com

---

## 📝 Licença

Este projeto foi desenvolvido como parte do MBA DevXpert e é destinado para fins educacionais.

---

## 🙏 Agradecimentos

- Desenvolvedores.io - MBA DevXpert
- Comunidade .NET Brasil

---

