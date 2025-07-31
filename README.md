# Sistema Gerenciamento de Tickets em C#

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white)

---

## 📄 Descrição do Projeto

Uma aplicação de console desenvolvida em C# para gerenciar a entrega de tickets para funcionários. O sistema permite o cadastro, edição e inativação de funcionários e tickets, além de gerar relatórios detalhados sobre a distribuição de tickets em um período específico.

A aplicação foi estruturada em camadas, tentando ao máximo separar a responsabilidade de cada uma.

**Obs:** Este projeto foi feito sem conhecimento prévio de C#, o que fiz aqui foi com base de 1 semana de estudo sobre a linguagem e pesquisas de como utilizar o EntityFrameworkCore.

## ✨ Funcionalidades

* **Manutenção de Funcionários:**
    * Cadastrar novo funcionário (com validação de CPF único e obrigatório).
    * Inativar funcionário (a exclusão não é permitida, apenas a alteração do status para 'I').
    * Listar todos os funcionários.
* **Gestão de Tickets:**
    * Cadastrar a entrega de tickets para um funcionário ativo.
    * Inativar um registro de entrega de ticket.
* **Relatórios:**
    * Gerar um relatório de tickets entregues em um período de datas.
---

## 🛠️ Tecnologias Utilizadas

* C#
* .NET versão 8.0
* Entity Framework Core - ORM para trabalhar com o banco de dados
* MySQL

---

## ⚙️ Arquitetura do Projeto

O projeto segue uma arquitetura em camadas:

- ``Program.cs``: camada de apresentação, responsável por interagir com o usuário e realizar chamadas para os serviços.
- ``Models/``: camada que contém as classes que representam as entidades do banco de dados.
- ``Repositories/``: camada de acesso ao banco de dados utilizando o Entity Framework Core.
- ``Services/``: camada da lógica de negócios, contém as regras e validações do sistema.
- ``Data/``: configuração da aplicação para se conectar ao banco de dados.

## 🚀 Como Executar o Projeto

Basta seguir os passos abaixos

### 1. Pré-requisitos

Certifique-se de que você tem instalado:

* [.NET SDK](https://dotnet.microsoft.com/download) versão 8.0
* [MySQL Server](https://dev.mysql.com/downloads/mysql/) versão 8.0

### 2. Configuração do Projeto

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/AspetereCoder/TicketApp.git
    cd TicketApp
    ```

2.  **Configure o Banco de Dados:**
    * Crie um banco de dados chamado ``ticketapp``
    * Abra o arquivo `appsettings.json` na raiz do projeto.
    * Edite a `ConnectionStrings:DefaultConnection` com as credenciais do seu MySQL local.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "server=localhost;port=SUA_PORTA;user=SEU_USUARIO;password=SUA_SENHA;database=ticketapp;"
    }
    ```
    * Certifique-se de que o banco de dados especificado (`ticketapp`) já existe ou que o usuário tem permissão para criá-lo.

3.  **Restaurar Pacotes e Aplicar Migrações:**
    * Use o terminal para instalar as dependências e aplicar as migrações, o que criará as tabelas no seu banco de dados.
    ```bash
    dotnet restore
    dotnet ef database update
    ```
    (Caso o comando `dotnet ef` não seja reconhecido, instale a ferramenta com `dotnet tool install --global dotnet-ef`).

### 3. Rodando a Aplicação

Com o banco de dados configurado, basta executar o seguinte comando:

```bash
dotnet run 
```

## ✍️ Autor

Feito por Pedro Augusto.

## LICENÇA

Este arquivo está sob a licença MIT. Para mais informações, consulte o arquivo [LICENSE](LICENSE)
