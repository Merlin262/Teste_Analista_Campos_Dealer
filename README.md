# Loja do Sr. Campos — Ecossistema de Vendas

Solução web desenvolvida como resposta ao desafio técnico da Campos Dealer. Evolui uma API legada de vendas para uma arquitetura MVC moderna em camadas, adicionando gestão de itens por venda, rastreabilidade de preços, ranking e interface web completa.

---

## Projetos da Solução

| Projeto | Tipo | Descrição |
|---|---|---|
| `TesteCamposDealer.Domain` | Class Library (.NET 4.8) | Entidades de domínio e interfaces dos repositórios |
| `TesteCamposDealer.Application` | Class Library (.NET 4.8) | Handlers CQRS, validators, behaviors e DTOs |
| `TesteCamposDealer.Infrastructure` | Class Library (.NET 4.8) | DbContext, migrations e implementação dos repositórios |
| `TesteCamposDealer.API` | ASP.NET MVC 5 (.NET 4.8) | Controllers, ViewModels, Mappers e configuração de DI |
| `TesteCamposDealer.Web` | ASP.NET MVC 5 (.NET 4.8) | Interface web — CRUD de vendas |
| `TesteCamposDealer.Tests` | xUnit (.NET 4.8) | Testes unitários dos handlers e repositórios |

---

## Pré-requisitos

- Visual Studio 2019 ou superior
- .NET Framework 4.8
- SQL Server Express ou LocalDB (`(localdb)\MSSQLLocalDB`)
- NuGet Package Restore habilitado

---

## Configuração e Execução

### 1. Clonar o repositório

```bash
git clone <url-do-repositorio>
```

### 2. Restaurar pacotes NuGet

No Visual Studio: clique com o botão direito na solução → **Restore NuGet Packages**.

### 3. Banco de dados

O projeto usa um banco legado com dados pré-existentes para simular o cenário real de atualização de schema sem perda de registros. São dois passos obrigatórios:

**3.1 Restaurar o backup**

Abra o SQL Server Management Studio (SSMS) e restaure o arquivo `DB/TesteCamposDealer.bak` para a instância `(localdb)\MSSQLLocalDB`.

Via SSMS: botão direito em **Databases → Restore Database** → selecione o arquivo `.bak`.

Ou via T-SQL:

```sql
RESTORE DATABASE TesteCamposDealer
    FROM DISK = 'caminho_completo\DB\TesteCamposDealer.bak'
    WITH REPLACE;
```

**3.2 Aplicar as migrations**

Com o banco restaurado, execute as migrations EF6 para evoluir o schema sem perda de dados. No **Package Manager Console** (View → Other Windows → Package Manager Console), com `TesteCamposDealer.Infrastructure` selecionado como _Default project_:

```powershell
Update-Database
```

As três migrations em `Infrastructure/Data/Migrations/` são aplicadas em ordem:
1. `FirstMigration` — converte PKs de `INT` para `UNIQUEIDENTIFIER`, cria `VendaItem` e `ProdutoPrecoHistorico`, migra os dados existentes
2. `RemoveLegacyId` — remove colunas legadas após a migração de dados
3. `FixVlrTotalToDec` — corrige o tipo de `vlrTotal` para `DECIMAL(18,2)`

> Essa abordagem foi escolhida deliberadamente para simular um banco legado com dados reais que precisava ser atualizado sem perda de registros.

Para usar SQL Server Express ou remoto, altere a connection string em `API/Web.config`:

```xml
<add name="TesteCamposDealerConnectionString"
     connectionString="Data Source=SEU_SERVIDOR;Initial Catalog=TesteCamposDealer;Integrated Security=True"
     providerName="System.Data.SqlClient" />
```

### 4. Executar

Defina `TesteCamposDealer.API` e `TesteCamposDealer.Web` como projeto de inicialização e pressione **F5**.

---

## Funcionalidades

- **Clientes** — CRUD completo com paginação ordenada por nome
- **Produtos** — CRUD com registro automático de histórico de preço ao alterar valor
- **Vendas** — CRUD com lista de itens, cálculo automático de totais e captura do preço do produto no momento da venda
- **Ranking** — vendas ordenadas por valor decrescente na tela de listagem
- **Vendas por cliente** — listagem filtrada por cliente
- **Formulário dinâmico** — adição/remoção de itens em tempo real com cálculo de total no frontend

---

## Testes Unitários

Para executar: abra o **Test Explorer** (Test → Test Explorer) e clique em **Run All**.

São **69 testes** cobrindo todos os handlers e repositórios.

---

## Arquitetura

![Diagrama de Entidades](docs/CamposDealerDiagram.png)

```
┌──────────────────────────────────┐
│      TesteCamposDealer.Web       │
│                                  │
│  Controllers / ViewModels        │
│  Views / Services (ApiClient)    │
└────────────────┬─────────────────┘
                 │ HTTP
                 ▼
┌──────────────────────────────────┐
│      TesteCamposDealer.API       │
│                                  │
│  Controllers / Mappers           │
│  Filters / DI                    │
└──────────┬──────────┬────────────┘
           │          │
           ▼          ▼
┌──────────────┐  ┌───────────────────────────────────┐
│ Application  │  │          Infrastructure           │
│              │  │                                   │
│ Handlers     │  │  AppDbContext / Migrations        │
│ Validators   │  │  ClienteRepository                │
│ Behaviors    │  │  ProdutoRepository                │
│ DTOs         │  │  VendaRepository / UnitOfWork     │
└──────┬───────┘  └─────────────────┬─────────────────┘
       │                            │
       └────────────┬───────────────┘
                    ▼
       ┌────────────────────────┐
       │         Domain         │
       │                        │
       │  Entities              │
       │  (Cliente, Produto,    │
       │   Venda, VendaItem,    │
       │   ProdutoPrecoHist.)   │
       │                        │
       │  Interfaces            │
       │  (IUnitOfWork,         │
       │   IClienteRepo, ...)   │
       └────────────┬───────────┘
                    │
            ┌───────▼───────┐
            │  SQL Server   │
            │  (LocalDB)    │
            └───────────────┘
```

---

## Estrutura de Pastas

```
TesteAnalistaCamposDealer/
├── Domain/                     ← TesteCamposDealer.Domain (Class Library)
│   ├── Entities/               ← Cliente, Produto, Venda, VendaItem, ProdutoPrecoHistorico
│   └── Interfaces/             ← IUnitOfWork, IClienteRepository, IProdutoRepository, IVendaRepository
│
├── Application/                ← TesteCamposDealer.Application (Class Library)
│   ├── Behaviors/              ← ValidationBehavior (pipeline MediatR)
│   ├── Common/                 ← PagedResult
│   ├── Dto/                    ← VendaItemRequest
│   ├── Exceptions/             ← NotFoundException, ValidationException
│   ├── Handlers/               ← Cliente/, Produto/, Venda/ (Commands + Queries)
│   └── Validators/             ← CreateCliente, UpdateCliente, ...
│
├── Infrastructure/             ← TesteCamposDealer.Infrastructure (Class Library)
│   └── Migrations/         ← EF6 Code First migrations (FirstMigration, RemoveLegacyId, FixVlrTotalToDec)
│   └── Repositories/           ← ClienteRepository, ProdutoRepository, VendaRepository, UnitOfWork
│
├── API/                        ← TesteCamposDealer.API (ASP.NET Web API 2)
│   ├── App_Start/              ← BundleConfig, FilterConfig, RouteConfig, WebApiConfig
│   ├── Controllers/            ← ClienteController, ProdutoController, VendaController
│   ├── Filters/                ← GlobalExceptionFilter
│   └── Mappers/                ← ViewModelMappers (usa ViewModels definidos em Web)
│
├── TesteCamposDealer.Web/      ← Interface web (ASP.NET MVC 5)
│   ├── Controllers/
│   ├── ViewModels/
│   └── Views/                  ← Cliente/, Produto/, Venda/, Shared/
│
├── TesteCamposDealer.Tests/    ← xUnit
│   ├── Handlers/
│   ├── Helpers/                ← MockDbSetHelper (infraestrutura async EF6)
│   └── Repositories/
│
├── DB/                         ← Backup do banco legado
│   └── TesteCamposDealer.bak   ← Restaurar antes de aplicar as migrations
│
└── docs/
    ├── historias-de-usuario.md
    ├── documentacao-tecnica.md
    ├── CamposDealer.postman_collection.json
    └── CamposDealerDiagram.png
```

---

## Documentação Adicional

| Arquivo | Conteúdo |
|---|---|
| [docs/documentacao-tecnica.md](docs/documentacao-tecnica.md) | Stack, arquitetura, modelagem, rotas, decisões técnicas e autoavaliação |
| [docs/historias-de-usuario.md](docs/historias-de-usuario.md) | 9 histórias de usuário em 4 épicos com critérios de aceite |
| [docs/CamposDealerDiagram.png](docs/CamposDealerDiagram.png) | Diagrama de entidades do banco de dados |

---

## Decisões Técnicas

- **.NET Framework 4.6 → 4.8** — O 4.8 é a versão LTS mais recente do .NET Framework, com suporte estendido da Microsoft e sem impacto de compatibilidade sobre os demais pacotes utilizados.
- **LINQ to SQL → Entity Framework 6** — A migração para EF6 Code First viabilizou versionamento do schema via migrations, mapeamento completo das entidades (`Venda → VendaItem → Produto`), o padrão Repository + Unit of Work e queries totalmente assíncronas.
- **UUID v7** — IDs gerados na aplicação, ordenados cronologicamente, sem fragmentação de índice
- **DECIMAL(18,2)** — substituiu `float` do banco legado para evitar erros de arredondamento monetário
- **CQRS com MediatR** — separa leitura de escrita sem dois bancos de dados; facilita testes e extensibilidade
- **ValidationBehavior** — pipeline MediatR que valida commands antes de chegarem ao handler; erros mapeados para `ModelState` e exibidos no formulário
- **Lazy loading desabilitado** — todas as queries usam `Include()` explícito para evitar o problema N+1

Veja [docs/documentacao-tecnica.md](docs/documentacao-tecnica.md) para detalhes completos.
