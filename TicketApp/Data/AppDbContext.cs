using TicketApp.Models;
using Microsoft.EntityFrameworkCore;

namespace TicketApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<Funcionario> Employees { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    // Construtor que recebe o DbContextOptions
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // torna o cpf um campo unico
        modelBuilder.Entity<Funcionario>()
            .HasIndex(employee => employee.CPF)
            .IsUnique();
        
        // configuração de chave estrangeira
        modelBuilder.Entity<Ticket>()
            .HasOne(ticket => ticket.Funcionario)
            .WithMany(employee => employee.Tickets)
            .HasForeignKey(ticket => ticket.FuncionarioId);
        
        base.OnModelCreating(modelBuilder);
    }
}