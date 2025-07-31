using TicketApp.Data;
using TicketApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using TicketApp.Repositories;

namespace TicketApp.Services;

public class TicketService
{
    // classe de validação das operações relacionadas a tickets
    
    private readonly AppDbContext _context;
    private readonly TicketRepository _ticketRepository;
    private readonly FuncionarioService _funcionarioService;

    public TicketService(AppDbContext context)
    {
        _context = context;
        _ticketRepository = new TicketRepository(context);
        _funcionarioService = new FuncionarioService(context);
    }

    public Ticket PegarTicket(int id)
    {
        return _ticketRepository.PegarPorId(id);
    }
    
    public List<Ticket> PegarTickets()
    {
        return _ticketRepository.PegarTickets();
    }
    
    public bool CadastrarTicket(string cpfFuncionario, int qtdTickets, DateTime dataHora)
    {
        var funcionario = _funcionarioService.PegarPorCpf(cpfFuncionario);

        // checa se o funcionário existe
        if (funcionario == null)
        {
            Console.WriteLine("\nFuncionário não encontrado.");
            return false;
        }
        
        // checa se o funcionário está inativo
        if (funcionario.Situacao == 'I')
        {
            Console.WriteLine("\nImpossivel cadastrar ticket em funcionário inativo.");
            return false;
        }
        
        // checa se a quantidade de tickets é válida
        if (qtdTickets <= 0)
        {
            Console.WriteLine("\nQuantidade de tickets inválida.");
            return false;
        }

        var novoTicket = new Ticket
            { Quantidade = qtdTickets, DataEntrega = dataHora};
        
        novoTicket.FuncionarioId = funcionario.Id; // linkando o ID do funcionário
        _ticketRepository.Adicionar(novoTicket);

        return true; // indica que o ticket foi cadastrado com sucesso
    }

    public bool InativarTicket(int id)
    {
        var ticket = PegarTicket(id);

        // checa pelo ticket existente
        if (ticket == null)
        {
            Console.WriteLine("\nTicket não encontrado.");
            return false;
        }

        // checa se o ticket já está inativo.
        if (ticket.Situacao == 'I')
        {
            Console.WriteLine("Ticket já está inativo.");
            return false;
        }
        
        // marcando o ticket como inativo.
        ticket.Situacao = 'I';
        _ticketRepository.Atualizar(ticket);

        return true; // indica que o ticket foi inativado com sucesso
    }

    public void GerarRelatorioTickets(DateTime initialDate, DateTime finalDate)
    {
        Console.WriteLine($"\n---Relatório de Tickets Entregues de {initialDate:dd/MM/yyyy} a {finalDate:dd/MM/yyyy}---");
        
        // ordenando os tickets conforme o periodo
        var tickets = _context.Tickets.Include(t => t.Funcionario) // inclui o campo Funcionário 
            .Where(t => t.DataEntrega >= initialDate && t.DataEntrega <= finalDate) // pega os tickes que estão entre a data
            .OrderBy(t => t.DataEntrega) // ordena pela data de recebimento
            .ToList();

        // se não encontrar nenhum ticket, mostra uma mensagem de erro
        if (tickets.Count == 0)
        {
            Console.WriteLine("\nNenhum ticket encontrado para o periodo informado.");
            return;
        }

        int totalTickets = 0;

        // listando os tickets
        Console.WriteLine();
        foreach (var ticket in tickets)
        {
            MostrarTicket(ticket);
            totalTickets += ticket.Quantidade;
        }
        
        Console.WriteLine($" - Total de tickets entregue no periodo: {totalTickets}");
    }

    public void MostrarTicket(Ticket ticket)
    {
        string cpfFormatado = Utils.Utils.FormatarCpf(ticket.Funcionario.CPF);
        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine($"| Ticket ID: {ticket.Id} - Situação: {ticket.Situacao}");
        Console.WriteLine($"| Dono: {ticket.Funcionario.Nome} - CPF: {cpfFormatado}");
        Console.WriteLine($"| Data de entrega: {ticket.DataEntrega:dd/MM/yyyy HH:mm:ss}");
        Console.WriteLine($"| Quantidade: {ticket.Quantidade}");
        Console.WriteLine("-------------------------------------------------------");
    }
}