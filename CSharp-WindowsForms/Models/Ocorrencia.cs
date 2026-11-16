namespace AlertaEnchentes.Desktop.Models;

public class Ocorrencia
{
    public int Id { get; set; }
    public string Local { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string NivelRisco { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Status { get; set; } = string.Empty;
}

/// <summary>Dados enviados à API no POST /api/ocorrencias.</summary>
public class NovaOcorrencia
{
    public string Local { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string NivelRisco { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
}
