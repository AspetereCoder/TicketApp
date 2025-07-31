using System.Text.RegularExpressions;

namespace TicketApp.Utils;

public static class Utils
{
    // classe com funções utilitárias
    
    /// <summary>
    /// Formata a string de um cpf cru para o formato xxx.xxx.xxx-xx
    /// </summary>
    /// <param name="cpfCru">string que representa o cpf</param>
    /// <returns>uma nova string para o cpf formatado</returns>
    public static string FormatarCpf(string cpfCru)
    {
        string cpfFormatado = string.Format(@"{0:000\.000\.000\-00}", long.Parse(cpfCru));
        return cpfFormatado;
    }
    
    /// <summary>
    /// Checa se a string passada como argumento pode ser convertida para um inteiro.
    /// </summary>
    /// <param name="qtd">input pego com o usuário</param>
    /// <returns>verdadeiro se conseguir converter, caso contrário retorna falso e exibe uma mensagem de erro</returns>
    public static bool ValidarQtdTickets(string qtd)
    {
        if (!int.TryParse(qtd, out int qtdTickets))
        {
            Console.WriteLine("\nQuantidade inválida.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checa se é possivel converter a string em um id (int)
    /// </summary>
    /// <param name="inputId">input do id pego com o usuário</param>
    /// <returns>verdadeiro se conseguir converter, caso contrário falso e exibe uma mensagem de erro</returns>
    public static bool ValidarInputId(string inputId)
    {
        if (!int.TryParse(inputId, out int id))
        {
            Console.WriteLine("\nID inválido.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checa se a data inserida é valida no formato dd/mm/aaaa
    /// </summary>
    /// <param name="dataString">input do data pega com o usuário</param>
    /// <param name="dataConvertida"></param>
    /// <returns>verdadeiro se conseguir converter, caso contrário falso e exibe uma mensagem de erro</returns>
    public static bool ValidaData(string dataString, out DateTime dataConvertida)
    {
        if (!DateTime.TryParseExact(dataString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dataConvertida))
        {
            Console.WriteLine("\nData inválida.");
            return false;
        }

        return true;
    }
    
    /// <summary>
    /// Checa se a hora inserida é valida no formato hh:mm 
    /// </summary>
    /// <param name="horaString">input do hora pega com o usuário</param>
    /// <param name="horaConvertida"></param>
    /// <returns>verdadeiro se conseguir converter, caso contrário falso e exibe uma mensagem de erro</returns>
    public static bool ValidaHora(string horaString, out DateTime horaConvertida)
    {
        if (!DateTime.TryParseExact(horaString, "HH:mm", null, System.Globalization.DateTimeStyles.None,
                out horaConvertida))
        {
            Console.WriteLine("\nHora inválida.");
            return false;
        }

        return true;
    }
    
    /// <summary>
    /// Checa se a escolha do usuário é valida.
    /// </summary>
    /// <param name="input">input do ação do usuário</param>
    /// <returns>verdadeiro se conseguir converter, caso contrário falso e exibe uma mensagem de erro</returns>
    public static bool ValidaEntradaUsuario(string input)
    {
        // checa se é possivel converter o input do usuário em um char
        if (!char.TryParse(input, out char escolha))
        {
            Console.WriteLine("\nAção inválida.");
            return false;
        }

        return true;
    }
    
    /// <summary>
    /// Checa se o nome inserido não possui numeros e caracteres especiais
    /// </summary>
    /// <param name="inputNome">input do nome do usuário</param>
    /// <returns>verdadeiro se passar na validação, caso contrário falso e exibe uma mensagem de erro</returns>
    public static bool ValidaNome(string inputNome)
    {
        // nome deve conter apenas caracteres minusculos ou maiusculos
        // e entre 1 a 40 caracteres
        if (!Regex.IsMatch(inputNome, @"^[a-zA-ZÀ-ÿ\s]{1,40}$"))
        {
            Console.WriteLine("\nNome inválido, deve conter somente letras, máximo de 40 caracteres.");
            return false;
        }

        return true;
    }
    
    /// <summary>
    /// Checa se o campo informado não é uma string nula ou vazia
    /// </summary>
    /// <param name="campo">nome ou cpf informado</param>
    /// <returns>verdadeiro se passar na validação, caso contrário falso</returns>
    public static bool CampoValido(string campo)
    {
        return !(String.IsNullOrWhiteSpace(campo));
    }
}