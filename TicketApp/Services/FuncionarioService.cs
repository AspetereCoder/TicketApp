using System.Text.RegularExpressions;
using TicketApp.Data;
using TicketApp.Models;
using TicketApp.Repositories;

namespace TicketApp.Services;

public class FuncionarioService
{
    // classe de validação das operações relacionadas a funcionários
    
    private readonly AppDbContext _context;
    private readonly FuncionarioRepository _funcionarioRepository;

    public FuncionarioService(AppDbContext context)
    {
        _context = context;
        _funcionarioRepository = new FuncionarioRepository(context);
    }
    
    public List<Funcionario> PegarFuncionarios()
    {
        return _funcionarioRepository.PegarFuncionarios();
    }

    public Funcionario PegarPorCpf(string cpf)
    {
        var employee = _funcionarioRepository.PegarPorCpf(cpf);
        
        return employee;
    }
    
    public bool AdicionarFuncionario(string nomeFuncionario, string cpfFuncionario)
    {
        bool cpfValido = ValidaCpf(cpfFuncionario);

        if (!cpfValido)
        {
            Console.WriteLine("\nCPF inválido.");
            return false;
        }
        
        var funcionarioExistente = _funcionarioRepository.PegarPorCpf(cpfFuncionario);
        
        // checa se já existe funcionário cadastrado com o cpf inserido
        if (funcionarioExistente != null)
        {
            Console.WriteLine("\nCPF já cadastrado.");
            return false;
        }
        
        // instancia um novo funcinario
        var novoFuncionario = new Funcionario { Nome = nomeFuncionario, CPF = cpfFuncionario };
        
        novoFuncionario.DataAlteracao = DateTime.Now;
        _funcionarioRepository.Adicionar(novoFuncionario);

        return true;
    }
    
    private void AtualizarFuncionario(Funcionario funcionario)
    {
        _funcionarioRepository.Atualizar(funcionario);
        _context.SaveChanges();
    }

    public bool AtivarFuncionario(string cpfFuncionario)
    {
        bool cpfValido = ValidaCpf(cpfFuncionario);

        if (!cpfValido)
        {
            Console.WriteLine("\nCPF inválido.");
            return false;
        }
        
        Funcionario funcionario = PegarPorCpf(cpfFuncionario);

        if (funcionario == null)
        {
            Console.WriteLine("\nFuncionário não encontrado.");
            return false;
        }

        // checa se o funcionário já está ativo
        if (funcionario.Situacao == 'A')
        {
            Console.WriteLine("\nFuncionário já está ativo.");
            return false;
        }

        funcionario.Situacao = 'A';
        funcionario.DataAlteracao = DateTime.Now;
        _funcionarioRepository.Atualizar(funcionario);
        
        return true; // indica que o funcionário foi ativado com sucesso
    }
    
    public bool InativarFuncionario(string cpfFuncionario)
    {
        bool cpfValido = ValidaCpf(cpfFuncionario);

        if (!cpfValido)
        {
            Console.WriteLine("\nCPF inválido.");
            return false;
        }
        
        var funcionarioExistente = PegarPorCpf(cpfFuncionario);

        if (funcionarioExistente == null)
        {
            Console.WriteLine("\nFuncionário não cadastrado.");
            return false;
        }
        
        // checa se o funcionário já está inativo
        if (funcionarioExistente.Situacao == 'I')
        {
            Console.WriteLine("\nFuncionário já está inativo.");
            return false;
        }

        // muda a situação do funcionário para inativa
        funcionarioExistente.Situacao = 'I';
        funcionarioExistente.DataAlteracao = DateTime.Now;
        
        AtualizarFuncionario(funcionarioExistente);
        
        return true; // indica que o funcionário foi inativado com sucesso
    }

    private bool ValidaCpf(string cpf)
    {
        // deve conter somente números e possuir 11 caracteres
        bool cpfValido = Regex.IsMatch(cpf, @"^\d{11}$");

        if (!cpfValido)
        {
            return false;
        }
        
        // Verifica se todos os dígitos são iguais
        if (cpf.Distinct().Count() == 1)
        {
            return false;
        }
        
        // cálculo do primeiro dígito verificador
        int soma = 0;
        for (int i = 0; i < 9; i++)
        {
            soma += (int)(cpf[i] - '0') * (10 - i);
        }
        int primeiroDigito = (soma * 10) % 11;
        if (primeiroDigito == 10 || primeiroDigito == 11)
        {
            primeiroDigito = 0;
        }
        
        // cálculo do segundo dígito verificador
        soma = 0;
        for (int i = 0; i < 10; i++)
        {
            soma += (int)(cpf[i] - '0') * (11 - i);
        }
        int segundoDigito = (soma * 10) % 11;
        if (segundoDigito == 10 || segundoDigito == 11)
        {
            segundoDigito = 0;
        }
        
        // Verifica se os dígitos verificadores estão corretos
        return cpf[9] == (char)(primeiroDigito + '0') && cpf[10] == (char)(segundoDigito + '0');
    }
}