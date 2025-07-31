using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketApp.Models;

public class Ticket
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int FuncionarioId { get; set; }
    
    [Required]
    public int Quantidade { get; set; }
    
    [Required, StringLength(1)]
    public char Situacao { get; set; } = 'A'; // salvo como ativo por padrão
    
    [Required]
    public DateTime DataEntrega { get; set; }
    
    [ForeignKey("EmployeeId")]
    [Required]
    public Funcionario Funcionario { get; set; }
    
}