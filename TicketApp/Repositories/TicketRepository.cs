using Microsoft.EntityFrameworkCore;
using TicketApp.Data;
using TicketApp.Models;

namespace TicketApp.Repositories;

public class TicketRepository
{
    // classe de acesso final ao banco de dados
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Ticket> PegarTickets()
    {
        return _context.Tickets.Include(t => t.Funcionario).ToList();
    }
    
    public Ticket PegarPorId(int id)
    {
        return _context.Tickets.Find(id);
    }

    public void Adicionar(Ticket ticket)
    {
        _context.Tickets.Add(ticket);
        _context.SaveChanges();
    }

    public void Atualizar(Ticket ticket)
    {
        _context.Tickets.Update(ticket);
        _context.SaveChanges();
    }
    
    
}