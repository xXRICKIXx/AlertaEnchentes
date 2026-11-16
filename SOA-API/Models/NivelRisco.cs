namespace AlertaEnchentes.Api.Models;

public static class NivelRisco
{
    public const string Baixo = "Baixo";
    public const string Medio = "Médio";
    public const string Alto = "Alto";

    /// <summary>Normaliza a entrada (ex.: "alto", "MEDIO") ou devolve null se inválida.</summary>
    public static string? Normalizar(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor)) return null;

        return valor.Trim().ToLowerInvariant().Replace("é", "e") switch
        {
            "baixo" => Baixo,
            "medio" => Medio,
            "alto" => Alto,
            _ => null
        };
    }
}
