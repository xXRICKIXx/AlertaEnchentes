namespace AlertaEnchentes.Api.Models;

public static class StatusOcorrencia
{
    public const string Aberta = "Aberta";
    public const string EmAcompanhamento = "Em acompanhamento";
    public const string Resolvida = "Resolvida";

    public static string? Normalizar(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor)) return null;

        return valor.Trim().ToLowerInvariant() switch
        {
            "aberta" => Aberta,
            "em acompanhamento" => EmAcompanhamento,
            "resolvida" => Resolvida,
            _ => null
        };
    }
}
