using AlertaEnchentes.Desktop.Models;

namespace AlertaEnchentes.Desktop.Services;

/// <summary>Validação dos dados digitados antes de enviar à API.</summary>
public static class Validador
{
    public static List<string> Validar(NovaOcorrencia o)
    {
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(o.Local))
            erros.Add("Informe o local da ocorrência.");
        else if (o.Local.Trim().Length > 120)
            erros.Add("O local deve ter no máximo 120 caracteres.");

        if (string.IsNullOrWhiteSpace(o.Tipo))
            erros.Add("Selecione o tipo da ocorrência.");

        if (!NivelRisco.Niveis.Contains(o.NivelRisco))
            erros.Add("Selecione o nível de risco (Baixo, Médio ou Alto).");

        var tamanho = o.Descricao?.Trim().Length ?? 0;
        if (tamanho < 5 || tamanho > 500)
            erros.Add("A descrição deve ter entre 5 e 500 caracteres.");

        return erros;
    }

    public static List<string> Validar(NovoAlerta a)
    {
        var erros = new List<string>();

        var tamanho = a.Mensagem?.Trim().Length ?? 0;
        if (tamanho < 5 || tamanho > 300)
            erros.Add("A mensagem do alerta deve ter entre 5 e 300 caracteres.");

        if (!NivelRisco.Niveis.Contains(a.NivelRisco))
            erros.Add("Selecione o nível de risco do alerta.");

        return erros;
    }
}
