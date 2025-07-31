using System.ComponentModel.DataAnnotations;

namespace TicketApp.Models;

public class Funcionario
{
    [Key]
    [Required]
    public int Id { get; set; }
    
    [Required, MaxLength(40, ErrorMessage = "Nome não deve possuir mais de 40 caracteres")]
    public string Nome { get; set; }
    
    [Required, StringLength(11)]
    public string CPF { get; set; }

    [Required, StringLength(1)]
    public char Situacao { get; set; } = 'A'; // salvo como ativo por padrão
    
    [Required]
    public DateTime DataAlteracao { get; set; }
    
    // tickets do funcionário
    public ICollection<Ticket> Tickets { get; set; }
}