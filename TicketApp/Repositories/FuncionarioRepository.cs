using TicketApp.Data;
using TicketApp.Models;
using Microsoft.EntityFrameworkCore;

namespace TicketApp.Repositories;

public class FuncionarioRepository
{
    // classe de acesso final ao banco de dados
    
    private readonly AppDbContext _context;

    public FuncionarioRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public List<Funcionario> PegarFuncionarios()
    {
        return _context.Employees.Include(e => e.Tickets).ToList();
    }

    public Funcionario PegarPorCpf(string cpf)
    {
        return _context.Employees.FirstOrDefault(e => e.CPF == cpf);
    }

    public void Adicionar(Funcionario funcionario)
    {
        _context.Employees.Add(funcionario);
        _context.SaveChanges();
    }

    public void Atualizar(Funcionario funcionario)
    {
        _context.Employees.Update(funcionario);
        _context.SaveChanges();
    }
    
}