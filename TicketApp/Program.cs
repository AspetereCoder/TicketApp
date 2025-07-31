using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TicketApp.Data;
using TicketApp.Services;

namespace TicketApp;

class Program
{
    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        
        IConfiguration configuration = builder.Build();
        
        string stringConexao = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(stringConexao))
        {
            Console.WriteLine("Erro: string de conexão 'Default Connection' não encontrada ou está vazia em appsettings.json");
            return;
        }
        
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
        
        var dbContext = new AppDbContext(optionsBuilder.Options);
        var fService = new FuncionarioService(dbContext);
        var tService = new TicketService(dbContext);
        
        dbContext.Database.EnsureCreated();
        
        bool appRunning = true;
        char userChoice;
        
        Console.Clear();
        
        while (appRunning)
        {
            Console.WriteLine("\n----Sistema de Gerenciamento de Tickets Refeições----");
            Console.WriteLine("## Opções:");
            Console.WriteLine("0 - Encerrar Execução");
            Console.WriteLine("1 - Gerenciar Funcionários");
            Console.WriteLine("2 - Gerenciar Tickets");
            Console.Write("Ação: ");

            string inputUsuario = Console.ReadLine() ?? string.Empty;

            // impede que o usuário não digite nada ou mais que um caractere
            if (!Utils.Utils.ValidaEntradaUsuario(inputUsuario))
            {
                continue;
            }
            
            userChoice = char.Parse(inputUsuario);

            switch (userChoice)
            {
                case '0':
                    appRunning = false;
                    break;
                case '1':
                    GerenciarFuncionarios(fService);
                    break;
                case '2':
                    GerenciarTickets(tService, fService);
                    break;
                default:
                    Console.WriteLine("\nAção inválida.");
                    break;
            }
            
        }
        
        Console.WriteLine("\nExecução encerrada.");
    }

    static void ListarFuncionarios(FuncionarioService fService)
    {
        var funcionarios = fService.PegarFuncionarios();

        if (funcionarios.Count() == 0)
        {
            Console.WriteLine("\nNenhum funcionário cadastrado.");
            return;
        }
        
        Console.WriteLine("\n----------------------------------------------------------------------------------------------------");
        foreach (var funcionario in funcionarios)
        {
            string cpfFormatado = Utils.Utils.FormatarCpf(funcionario.CPF);
            Console.WriteLine($"| Id: {funcionario.Id} - Nome: {funcionario.Nome} - CPF: {cpfFormatado} - Status: {funcionario.Situacao} - Data: {funcionario.DataAlteracao}");
        }

        Console.WriteLine("-----------------------------------------------------------------------------------------------------");
    }

    static void CadastrarFuncionario(FuncionarioService service)
    {
        Console.WriteLine("\n------------------------------------------");
        Console.Write("| Nome: ");
        string nome = Console.ReadLine() ?? string.Empty;
        
        Console.Write("| CPF (apenas números): ");
        string cpf = Console.ReadLine() ?? string.Empty; 
        Console.WriteLine("------------------------------------------");
        
        // impede de que o cpf e o nome sejam strings vazias
        if (!Utils.Utils.CampoValido(nome) || !Utils.Utils.CampoValido(cpf))
        {
            Console.WriteLine("\nNome e CPF são obrigatórios.");
            return;
        }

        // validação do nome -> apenas caracteres válidos
        if (!Utils.Utils.ValidaNome(nome))
        {
            return;
        }
        
        bool funcionarioCadastrado = service.AdicionarFuncionario(nome, cpf);
        
        if (funcionarioCadastrado)
        {
            Console.WriteLine("\nFuncionário Cadastrado.\n");
        }
    }
    
    static void InativarFuncionario(FuncionarioService fService)
    {
        Console.WriteLine("\n---------------------------------------");
        Console.Write("| CPF (somente numeros): ");
        string cpf = Console.ReadLine() ?? string.Empty;
        
        Console.WriteLine("---------------------------------------");
        
        // validação do cpf digitado
        if (!Utils.Utils.CampoValido(cpf))
        {
            Console.WriteLine("\nCPF inválido.");
            return;
        }

        bool funcionarioInativado = fService.InativarFuncionario(cpf);

        if (funcionarioInativado)
        {
            Console.WriteLine("\nFuncionário Inativado.");
        }
    }

    static void AtivarFuncionario(FuncionarioService fService)
    {
        // pega cpf
        Console.WriteLine("\n---------------------------------------");
        Console.Write("| CPF (somente números): ");
        string cpf = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("---------------------------------------");
        
        // validação
        if (!Utils.Utils.CampoValido(cpf))
        {
            Console.WriteLine("\nCPF inválido.");
            return;
        }
        
        bool funcionarioAtivado = fService.AtivarFuncionario(cpf);

        if (funcionarioAtivado)
        {
            Console.WriteLine("\nFuncionario ativado com sucesso.");
        }
    }
   

    static void ListarTickets(TicketService tService)
    {
        var tickets = tService.PegarTickets();

        // checa se não existe nenhum ticket cadastrado
        if (tickets.Count() == 0)
        {
            Console.WriteLine("\nNenhum ticket cadastrado.");
            return;
        }
        
        Console.WriteLine();
        foreach (var ticket in tickets)
        {
            tService.MostrarTicket(ticket);
        }
    }

    static void ListarTicketsPorFuncionario(TicketService tService, FuncionarioService fService)
    {
        Console.WriteLine();
        
        // itera sobre cada funcionário e pega a sua quantidade de tickets
        var funcionarios = fService.PegarFuncionarios();
        foreach (var funcionario in funcionarios)
        {
            var ticketsFuncionario = funcionario.Tickets.ToList();
            int totalTicketsFuncionario = 0;
            
            foreach (var ticket in ticketsFuncionario)
            {
                totalTicketsFuncionario += ticket.Quantidade;
            }
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"| Funcionário: {funcionario.Nome}");
            Console.WriteLine($"| Ticket: {totalTicketsFuncionario}");
            Console.WriteLine("----------------------------------------");
        }
    }

    static void CadastrarTicket(TicketService tService)
    {
        // pega cpf do funcionario
        Console.WriteLine("\n-------------------------------------");
        Console.Write("| CPF (apenas números): ");
        string cpf = Console.ReadLine() ?? string.Empty;
        
        // validação do cpf
        if (!Utils.Utils.CampoValido(cpf))
        {
            Console.WriteLine("Cpf inválido.");
            return;
        }
        
        // pega quantidade de tickets
        Console.Write("| Qtd tickets: ");
        string qtdTicketsInput = Console.ReadLine() ?? string.Empty;
        
        if (!Utils.Utils.ValidarQtdTickets(qtdTicketsInput))
        {
            return;
        }
        
        int qtdTickets = int.Parse(qtdTicketsInput);
        
        // pega data de entrega do ticket
        Console.Write("| Data de entrega (dd/mm/aaaa): ");
        string data = Console.ReadLine() ?? string.Empty;
        
        // validação da data
        if (!Utils.Utils.ValidaData(data, out DateTime dataConvertidaDt))
        {
            return;
        }
        
        // pega hora da entrega do ticket
        Console.Write("| Hora da entrega (hh:mm): ");
        string hora = Console.ReadLine() ?? string.Empty;
        
        Console.WriteLine("------------------------------------");
        
        // validação da hora
        if (!Utils.Utils.ValidaHora(hora, out DateTime horaConvertidaDt))
        {
            return;
        }

        // data e hora da entrega do ticket formatada
        DateTime dataFormatada = new DateTime(dataConvertidaDt.Year, dataConvertidaDt.Month, dataConvertidaDt.Day, horaConvertidaDt.Hour,
            horaConvertidaDt.Minute, horaConvertidaDt.Second);
        
        bool ticketCadastrado = tService.CadastrarTicket(cpf, qtdTickets, dataFormatada);

        if (ticketCadastrado)
        {
            Console.WriteLine("\nTicket cadastrado com súcesso.");
        }
    }

    static void GerarRelatorioTickets(TicketService tService)
    {
        Console.WriteLine("\n--------------------------------------------------");
        Console.Write("| Insira a data inicial (dd/mm/aaaa): ");
        string dataInicial = Console.ReadLine() ?? string.Empty;
                    
        Console.Write("| Insira a data final (dd/mm/aaaa): ");
        string dataFinal = Console.ReadLine() ?? string.Empty;
        
        Console.WriteLine("-------------------------------------------------");
        
        if (!Utils.Utils.ValidaData(dataInicial, out DateTime dataInicialDt) || !Utils.Utils.ValidaData(dataFinal, out DateTime dataFinalDt))
        {
            return;
        }
        
        tService.GerarRelatorioTickets(dataInicialDt, dataFinalDt);
    }

    static void InativarTicket(TicketService ticketService)
    {
        Console.WriteLine("\n-----------------------");
        Console.Write("| ID do ticket: ");
        
        string idInput = Console.ReadLine() ?? string.Empty;

        // validando id digitado pelo usuário
        if (!Utils.Utils.ValidarInputId(idInput))
        {
            Console.WriteLine("-----------------------");
            return;
        }
        
        Console.WriteLine("-----------------------");
        
        int id = int.Parse(idInput);
        
        bool ticketInativado = ticketService.InativarTicket(id);

        // exibindo mensagem de sucesso ao cadastrar o ticket
        if (ticketInativado)
        {
            Console.WriteLine("\nTicket inativado com sucesso.");
        }
    }

    static void GerenciarFuncionarios(FuncionarioService fService)
    {
        bool shouldLeave = false;
        char userChoice;
        
        Console.Clear();
        while (!shouldLeave)
        {
            Console.WriteLine("\n-----Gerenciamento de Funcionários-----");
            Console.WriteLine("## Opções:");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine("1 - Listar Funcionáiros");
            Console.WriteLine("2 - Cadastrar Funcionário");
            Console.WriteLine("3 - Inativar Funcionário");
            Console.WriteLine("4 - Ativar Funcionário");
            Console.Write("Ação: ");

            string inputUsuario = Console.ReadLine() ?? string.Empty;
            
            // impedindo de prosseguir caso o usuário digite algo inválido.
            if (!Utils.Utils.ValidaEntradaUsuario(inputUsuario))
            {
                continue;
            }
            
            userChoice = char.Parse(inputUsuario);

            switch (userChoice)
            {
                case '0':
                    Console.Clear();
                    shouldLeave = true;
                    break;
                case '1':
                    ListarFuncionarios(fService);
                    break;
                case '2':
                    CadastrarFuncionario(fService);
                    break;
                case '3':
                    InativarFuncionario(fService);
                    break;
                case '4':
                    AtivarFuncionario(fService);
                    break;
                default:
                    Console.WriteLine("Opção Inválida.");
                    break;
            }
        }
    }

    static void GerenciarTickets(TicketService tService, FuncionarioService fService)
    {
        bool shouldLeave = false;
        char userChoice;
        
        Console.Clear();
        while (!shouldLeave)
        {
            Console.WriteLine("\n-----Gerenciamento de Tickets-----");
            Console.WriteLine("## Opções:");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine("1 - Listar Tickets");
            Console.WriteLine("2 - Cadastrar Ticket");
            Console.WriteLine("3 - Inativar Ticket");
            Console.WriteLine("4 - Relatório de Tickets");
            Console.WriteLine("5 - Relatório de Tickets por funcionário");
            Console.Write("Ação: ");

            string inputUsuario = Console.ReadLine() ?? string.Empty;

            // impedindo de prosseguir caso o usuário digite algo inválido.
            if (!Utils.Utils.ValidaEntradaUsuario(inputUsuario))
            {
                continue;
            }
            
            userChoice = char.Parse(inputUsuario);

            switch (userChoice)
            {
                case '0':
                    Console.Clear();
                    shouldLeave = true;
                    break;
                case '1':
                    ListarTickets(tService);
                    break;
                case '2':
                    CadastrarTicket(tService);
                    break;
                case '3':
                    InativarTicket(tService);
                    break;
                case '4':
                    GerarRelatorioTickets(tService);
                    break;
                case '5':
                    ListarTicketsPorFuncionario(tService, fService);
                    break;
                default:
                    Console.WriteLine("Ação inválida.");
                    break;
            }
        }
    }
    
}