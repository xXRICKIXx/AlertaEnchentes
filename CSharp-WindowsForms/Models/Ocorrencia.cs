namespace AlertaEnchentes.Desktop.Models;

/// <summary>Ocorrência devolvida pela API.</summary>
public class Ocorrencia
{
    public int Id { get; set; }
    public string Local { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string NivelRisco { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Status { get; set; } = string.Empty;

    // Regras do domínio expostas como propriedades somente leitura.
    public bool EhRiscoAlto => NivelRisco == Models.NivelRisco.Alto;
    public bool EstaResolvida => Status == "Resolvida";
}

/// <summary>
/// Dados enviados à API no POST /api/ocorrencias.
/// Os campos são encapsulados: todo valor atribuído é normalizado (trim)
/// e a própria classe sabe se está válida.
/// </summary>
public class NovaOcorrencia
{
    public const int MaxLocal = 120;
    public const int MinDescricao = 5;
    public const int MaxDescricao = 500;

    private string _local = string.Empty;
    private string _tipo = string.Empty;
    private string _nivelRisco = string.Empty;
    private string _descricao = string.Empty;

    public string Local { get => _local; set => _local = value?.Trim() ?? string.Empty; }
    public string Tipo { get => _tipo; set => _tipo = value?.Trim() ?? string.Empty; }
    public string NivelRisco { get => _nivelRisco; set => _nivelRisco = value?.Trim() ?? string.Empty; }
    public string Descricao { get => _descricao; set => _descricao = value?.Trim() ?? string.Empty; }

    /// <summary>Retorna a lista de problemas; vazia quando os dados são válidos.</summary>
    public List<string> Validar()
    {
        var erros = new List<string>();

        if (_local.Length == 0)
            erros.Add("Informe o local da ocorrência.");
        else if (_local.Length > MaxLocal)
            erros.Add($"O local deve ter no máximo {MaxLocal} caracteres.");

        if (_tipo.Length == 0)
            erros.Add("Selecione o tipo da ocorrência.");

        if (!Models.NivelRisco.Niveis.Contains(_nivelRisco))
            erros.Add("Selecione o nível de risco (Baixo, Médio ou Alto).");

        if (_descricao.Length < MinDescricao || _descricao.Length > MaxDescricao)
            erros.Add($"A descrição deve ter entre {MinDescricao} e {MaxDescricao} caracteres.");

        return erros;
    }
}
