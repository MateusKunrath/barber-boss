# BarberBoss

## Sobre o projeto

O **BarberBoss** é uma API REST desenvolvida com **.NET 8** para gerenciar faturamentos de uma barbearia. A aplicação permite cadastrar, consultar, atualizar e remover registros de cobrança, mantendo informações como barbeiro, cliente, serviço realizado, valor, data, observações, forma de pagamento e status do faturamento.

O projeto foi organizado em camadas, seguindo uma separação próxima aos princípios de **Domain-Driven Design (DDD)**. A camada de domínio concentra entidades, enums, contratos de repositórios e mensagens de relatórios; a camada de aplicação reúne casos de uso, validações, mapeamentos e geração de relatórios; a infraestrutura implementa o acesso a dados com **Entity Framework Core** e **MySQL**; e a API expõe os endpoints HTTP com documentação via **Swagger**.

Além do CRUD de faturamentos, a API gera relatórios mensais em **Excel** e **PDF**. Os relatórios são produzidos a partir dos faturamentos filtrados por mês, usando **ClosedXML** para planilhas e **PDFsharp/MigraDoc** para documentos PDF.

### Features

- **Cadastro de faturamentos**: registra cobranças com barbeiro, cliente, serviço, valor, data, forma de pagamento e status.
- **Consulta paginada e filtrada**: lista faturamentos com filtros por barbeiro, cliente e status, além de paginação e ordenação.
- **Busca por identificador**: recupera os dados completos de um faturamento pelo `id`.
- **Atualização e remoção**: permite editar e excluir faturamentos existentes.
- **Relatórios mensais**: exporta os faturamentos de um mês em arquivos `.xlsx` e `.pdf`.
- **Validações**: regras de entrada implementadas com **FluentValidation**.
- **Testes de unidade**: testes com **xUnit** e **Shouldly** para validar o comportamento dos validadores.
- **Swagger**: interface interativa para visualizar e testar os endpoints da API.

### Construído com

![badge-dot-net]
![badge-csharp]
![badge-mysql]
![badge-entity-framework]
![badge-swagger]
![badge-xunit]

## Getting Started

Siga os passos abaixo para executar o projeto localmente.

### Requisitos

- [.NET SDK 8.0][dot-net-sdk] ou superior.
- MySQL Server.
- Visual Studio 2022, JetBrains Rider, Visual Studio Code ou outra IDE com suporte a .NET.

### Instalação

1. Clone o repositório:

   ```sh
   git clone git@github.com:MateusKunrath/barber-boss.git
   ```

2. Acesse a pasta do projeto:

   ```sh
   cd barber-boss
   ```

3. Configure a connection string em `src/BarberBoss.Api/appsettings.Development.json`:

   ```json
   {
     "ConnectionStrings": {
       "Connection": "Server=localhost;Database=barberbossdb;Uid=root;Pwd=sua_senha;"
     }
   }
   ```

4. Crie o banco de dados no MySQL:

   ```sql
   CREATE DATABASE barberbossdb;
   ```

   Caso a tabela ainda não exista, crie a estrutura usada pela API:

   ```sql
   USE barberbossdb;

   CREATE TABLE Billings (
     Id CHAR(36) NOT NULL PRIMARY KEY,
     BarberName VARCHAR(255) NOT NULL,
     ClientName VARCHAR(255) NOT NULL,
     ServiceName VARCHAR(255) NOT NULL,
     Amount DECIMAL(18, 2) NOT NULL,
     Date DATETIME(6) NOT NULL,
     Notes TEXT NOT NULL,
     PaymentMethod INT NOT NULL,
     Status INT NOT NULL,
     CreatedAt DATETIME(6) NULL,
     UpdatedAt DATETIME(6) NULL
   );
   ```

5. Restaure as dependências:

   ```sh
   dotnet restore BarberBoss.slnx
   ```

6. Execute a API:

   ```sh
   dotnet run --project src/BarberBoss.Api/BarberBoss.Api.csproj
   ```

7. Abra o Swagger no navegador:

   ```text
   https://localhost:{porta}/swagger
   ```

   A porta será exibida no terminal ao iniciar a aplicação.

## Como testar

Para executar todos os testes do projeto:

```sh
dotnet test BarberBoss.slnx
```

Também é possível executar apenas o projeto de testes dos validadores:

```sh
dotnet test tests/Validators.Tests/Validators.Tests.csproj
```

Os testes atuais cobrem as regras de validação dos faturamentos usando **xUnit** e **Shouldly**.

## Endpoints principais

### Faturamentos

| Método   | Rota                 | Descrição                                              |
| -------- | -------------------- | ------------------------------------------------------ |
| `POST`   | `/api/Billings`      | Cadastra um faturamento.                               |
| `GET`    | `/api/Billings`      | Lista faturamentos com paginação, filtros e ordenação. |
| `GET`    | `/api/Billings/{id}` | Busca um faturamento pelo identificador.               |
| `PUT`    | `/api/Billings/{id}` | Atualiza um faturamento existente.                     |
| `DELETE` | `/api/Billings/{id}` | Remove um faturamento.                                 |

Exemplo de corpo para cadastro ou atualização:

```json
{
  "barberName": "João Silva",
  "clientName": "Carlos Souza",
  "serviceName": "Corte degradê",
  "amount": 45.9,
  "notes": "Cliente mensalista",
  "paymentMethod": 0,
  "status": 0,
  "date": "2026-06-17T14:30:00"
}
```

Valores aceitos para `paymentMethod`:

| Valor | Descrição                |
| ----- | ------------------------ |
| `0`   | Dinheiro                 |
| `1`   | Cartão de crédito        |
| `2`   | Cartão de débito         |
| `3`   | Transferência eletrônica |
| `4`   | Outro                    |

Valores aceitos para `status`:

| Valor | Descrição |
| ----- | --------- |
| `0`   | Pago      |
| `1`   | Cancelado |

Exemplo de listagem com filtros:

```text
GET /api/Billings?page=1&pageSize=10&barberName=João&status=0&orderBy=date&descending=true
```

Os valores aceitos para `orderBy` são `date` e `amount`.

### Relatórios

| Método | Rota                 | Formato |
| ------ | -------------------- | ------- |
| `GET`  | `/api/Reports/Excel` | `.xlsx` |
| `GET`  | `/api/Reports/Pdf`   | `.pdf`  |

Os endpoints de relatório recebem o mês pelo header `month`, no formato de data aceito por `DateOnly`, por exemplo `2026-06-01`. A API usa o ano e o mês informados para buscar todos os faturamentos daquele período.

Exemplo com `curl` para baixar o relatório em Excel:

```sh
curl -k -L -o report.xlsx -H "month: 2026-06-01" https://localhost:{porta}/api/Reports/Excel
```

Exemplo com `curl` para baixar o relatório em PDF:

```sh
curl -k -L -o report.pdf -H "month: 2026-06-01" https://localhost:{porta}/api/Reports/Pdf
```

Também é possível visualizar e executar esses endpoints pelo Swagger. Ao receber uma resposta `200 OK`, o arquivo será retornado para download; caso não existam faturamentos no mês informado, a API retorna `204 No Content`.

## Estrutura do projeto

```text
src/
  BarberBoss.Api/             # Controllers, Swagger, filtros e middlewares
  BarberBoss.Application/     # Casos de uso, validações, AutoMapper e relatórios
  BarberBoss.Communication/   # Requests, responses e enums usados pela API
  BarberBoss.Domain/          # Entidades, enums, recursos e contratos
  BarberBoss.Exception/       # Exceções e mensagens de erro
  BarberBoss.Infrastructure/  # DbContext, repositórios e configuração do MySQL

tests/
  CommonTestUtilities/        # Builders e utilitários para testes
  Validators.Tests/           # Testes de validação dos faturamentos
```

<!-- Links -->

[dot-net-sdk]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0

<!-- Badges -->

[badge-dot-net]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-csharp]: https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=fff&style=for-the-badge
[badge-mysql]: https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=fff&style=for-the-badge
[badge-entity-framework]: https://img.shields.io/badge/Entity%20Framework-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-swagger]: https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=000&style=for-the-badge
[badge-xunit]: https://img.shields.io/badge/xUnit-5E2B97?style=for-the-badge
