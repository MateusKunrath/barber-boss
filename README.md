# BarberBoss

## Sobre o projeto

O **BarberBoss** é uma API REST desenvolvida com **.NET 8** para gerenciar faturamentos de uma barbearia. A aplicação permite cadastrar, consultar, atualizar e remover registros de cobrança, mantendo informações como barbeiro, cliente, serviço realizado, valor, data, observações, forma de pagamento e status do faturamento.

O projeto foi organizado em camadas, seguindo uma separação próxima aos princípios de **Domain-Driven Design (DDD)**. A camada de domínio concentra entidades, enums, contratos de repositórios e mensagens de relatórios; a camada de aplicação reúne casos de uso, validações, mapeamentos, autenticação e geração de relatórios; a infraestrutura implementa o acesso a dados com **Entity Framework Core**, **MySQL**, migrations e segurança; e a API expõe os endpoints HTTP com autenticação via **JWT** e documentação via **Swagger**.

Além do CRUD de faturamentos, a API possui cadastro e autenticação de usuários, controle de acesso por perfil, atualização de perfil, exclusão de usuários e geração de relatórios mensais em **Excel** e **PDF**. Os relatórios são produzidos a partir dos faturamentos filtrados por mês, usando **ClosedXML** para planilhas e **PDFsharp/MigraDoc** para documentos PDF.

### Features

- **Cadastro de usuários**: registra usuários com nome, e-mail, senha criptografada e perfil de acesso.
- **Autenticação com JWT**: gera token de acesso para consumo dos endpoints protegidos.
- **Gerenciamento de usuários**: consulta, atualização, atualização de perfil e remoção de usuários autenticados.
- **Controle de acesso por perfil**: protege os relatórios para usuários com perfil `Admin`.
- **Cadastro de faturamentos**: registra cobranças com barbeiro, cliente, serviço, valor, data, forma de pagamento e status.
- **Consulta paginada e filtrada**: lista faturamentos com filtros por barbeiro, cliente e status, além de paginação e ordenação.
- **Busca por identificador**: recupera os dados completos de um faturamento pelo `id`.
- **Atualização e remoção**: permite editar e excluir faturamentos existentes.
- **Relatórios mensais**: exporta os faturamentos de um mês em arquivos `.xlsx` e `.pdf`.
- **Migrations**: versionamento e aplicação automática da estrutura do banco com **Entity Framework Core**.
- **Validações**: regras de entrada implementadas com **FluentValidation**.
- **Testes automatizados**: testes com **xUnit** e **Shouldly** para validadores, casos de uso e endpoints da Web API.
- **Swagger**: interface interativa para visualizar, autenticar e testar os endpoints da API.

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

3. Configure a connection string e as configurações do JWT em `src/BarberBoss.Api/appsettings.Development.json`:

   ```json
   {
     "ConnectionStrings": {
       "Connection": "Server=localhost;Database=barberbossdb;Uid=root;Pwd=sua_senha;"
     },
     "Settings": {
       "Jwt": {
         "SigningKey": "sua_chave_secreta_com_tamanho_seguro",
         "ExpiresMinutes": 1000
       }
     }
   }
   ```

4. Crie o banco de dados no MySQL:

   ```sql
   CREATE DATABASE barberbossdb;
   ```

   A estrutura das tabelas é controlada pelas migrations do **Entity Framework Core**. Ao iniciar a API, as migrations pendentes são aplicadas automaticamente no banco configurado.

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

Também é possível executar projetos de teste específicos:

```sh
dotnet test tests/Validators.Tests/Validators.Tests.csproj
dotnet test tests/UseCases.Test/UseCases.Test.csproj
dotnet test tests/WebApi.Test/WebApi.Test.csproj
```

Os testes atuais cobrem regras de validação, casos de uso e endpoints da Web API usando **xUnit** e **Shouldly**.

## Endpoints principais

Os endpoints protegidos exigem o header `Authorization` com o token JWT retornado na autenticação:

```text
Authorization: Bearer {token}
```

### Autenticação

| Método | Rota                | Descrição                      |
| ------ | ------------------- | ------------------------------ |
| `POST` | `/api/Authenticate` | Autentica um usuário existente. |

Exemplo de corpo para autenticação:

```json
{
  "email": "admin@barberboss.com",
  "password": "Senha@123"
}
```

### Usuários

| Método   | Rota              | Descrição                                  |
| -------- | ----------------- | ------------------------------------------ |
| `POST`   | `/api/Users`      | Cadastra um usuário.                       |
| `GET`    | `/api/Users`      | Busca o perfil do usuário autenticado.     |
| `PUT`    | `/api/Users`      | Atualiza o perfil do usuário autenticado.  |
| `GET`    | `/api/Users/{id}` | Busca um usuário pelo identificador.       |
| `PUT`    | `/api/Users/{id}` | Atualiza um usuário existente.             |
| `DELETE` | `/api/Users/{id}` | Remove um usuário.                         |

Exemplo de corpo para cadastro:

```json
{
  "name": "João Silva",
  "email": "joao@barberboss.com",
  "password": "Senha@123",
  "role": "Admin"
}
```

Exemplo de corpo para atualização de perfil:

```json
{
  "name": "João Silva",
  "email": "joao@barberboss.com"
}
```

Exemplo de corpo para atualização de usuário:

```json
{
  "name": "João Silva",
  "email": "joao@barberboss.com",
  "role": "User"
}
```

Valores aceitos para `role`:

| Valor   | Descrição              |
| ------- | ---------------------- |
| `User`  | Usuário comum          |
| `Admin` | Usuário administrador  |

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

Os endpoints de relatório exigem autenticação com perfil `Admin` e recebem o mês pela query string `month`, no formato de data aceito por `DateOnly`, por exemplo `2026-06-01`. A API usa o ano e o mês informados para buscar todos os faturamentos daquele período.

Exemplo com `curl` para baixar o relatório em Excel:

```sh
curl -k -L -o report.xlsx -H "Authorization: Bearer {token}" "https://localhost:{porta}/api/Reports/Excel?month=2026-06-01"
```

Exemplo com `curl` para baixar o relatório em PDF:

```sh
curl -k -L -o report.pdf -H "Authorization: Bearer {token}" "https://localhost:{porta}/api/Reports/Pdf?month=2026-06-01"
```

Também é possível visualizar e executar esses endpoints pelo Swagger. Ao receber uma resposta `200 OK`, o arquivo será retornado para download; caso não existam faturamentos no mês informado, a API retorna `204 No Content`.

## Estrutura do projeto

```text
src/
  BarberBoss.Api/             # Controllers, Swagger, filtros, middlewares e autenticação HTTP
  BarberBoss.Application/     # Casos de uso, validações, AutoMapper e relatórios
  BarberBoss.Communication/   # Requests, responses e enums usados pela API
  BarberBoss.Domain/          # Entidades, enums, recursos, segurança e contratos
  BarberBoss.Exception/       # Exceções e mensagens de erro
  BarberBoss.Infrastructure/  # DbContext, migrations, repositórios, JWT, criptografia e MySQL

tests/
  CommonTestUtilities/        # Builders e utilitários para testes
  Validators.Tests/           # Testes de validação
  UseCases.Test/              # Testes dos casos de uso
  WebApi.Test/                # Testes dos endpoints da API
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
