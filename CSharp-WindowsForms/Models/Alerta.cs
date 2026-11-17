namespace AlertaEnchentes.Desktop.Models;


public class Alerta
{
    public int Id { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public string NivelRisco { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Status { get; set; } = string.Empty;
}


public class NovoAlerta
{
    public const int MinMensagem = 5;
    public const int MaxMensagem = 300;

    private string _mensagem = string.Empty;
    private string _nivelRisco = string.Empty;

    public string Mensagem { get => _mensagem; set => _mensagem = value?.Trim() ?? string.Empty; }
    public string NivelRisco { get => _nivelRisco; set => _nivelRisco = value?.Trim() ?? string.Empty; }

    public List<string> Validar()
    {
        var erros = new List<string>();

        if (_mensagem.Length < MinMensagem || _mensagem.Length > MaxMensagem)
            erros.Add($"A mensagem do alerta deve ter entre {MinMensagem} e {MaxMensagem} caracteres.");

        if (!Models.NivelRisco.Niveis.Contains(_nivelRisco))
            erros.Add("Selecione o nível de risco do alerta.");

        return erros;
    }
}
