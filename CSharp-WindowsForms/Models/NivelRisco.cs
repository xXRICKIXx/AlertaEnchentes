namespace AlertaEnchentes.Desktop.Models;

public static class NivelRisco
{
    public const string Baixo = "Baixo";
    public const string Medio = "Médio";
    public const string Alto = "Alto";

    public static readonly string[] Niveis = { Baixo, Medio, Alto };

    private static readonly string[] PalavrasAlto =
    {
        "submers", "carro", "veículo", "veiculo", "interdit", "transbord", "enchente",
        "ilhad", "tomad", "correnteza", "idoso", "criança", "crianca", "energia"
    };

    private static readonly string[] PalavrasMedio =
    {
        "acúmulo", "acumulo", "bueiro", "entupid", "lent", "trânsito", "transito", "calçada", "calcada", "poça", "poca"
    };

    /// <summary>
    /// Identifica o nível de risco provável a partir da descrição digitada.
    /// É apenas uma sugestão: o usuário pode alterar antes de registrar.
    /// </summary>
    public static string Sugerir(string? descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao)) return Baixo;

        var texto = descricao.ToLowerInvariant();

        if (PalavrasAlto.Any(texto.Contains)) return Alto;
        if (PalavrasMedio.Any(texto.Contains)) return Medio;
        return Baixo;
    }
}
