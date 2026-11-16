namespace AlertaEnchentes.Desktop.Models;

public class Alerta
{
    public int Id { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public string NivelRisco { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Status { get; set; } = string.Empty;
}

/// <summary>Dados enviados à API no POST /api/alertas.</summary>
public class NovoAlerta
{
    public string Mensagem { get; set; } = string.Empty;
    public string NivelRisco { get; set; } = string.Empty;
}
