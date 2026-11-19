namespace AlertaEnchentes.Desktop.Models;

public static class NivelRisco
{
    public const string Baixo = "Baixo";
    public const string Medio = "Médio";
    public const string Alto = "Alto";

    public static readonly string[] Niveis = { Baixo, Medio, Alto };

    /// <summary>
    /// Identifica o nível de risco provável a partir da descrição digitada.
    /// É apenas uma sugestão: o usuário pode alterar antes de registrar.
    /// As regras são avaliadas em ordem de prioridade: alto, depois médio.
    /// </summary>
    public static string Sugerir(string? descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao)) return Baixo;

        var texto = descricao.ToLowerInvariant();

        // Polimorfismo: todas as regras são tratadas pela classe-base,
        // mas cada classe derivada executa sua própria implementação.
        RegraClassificacaoRisco[] regras =
        {
            new RegraRiscoAlto(),
            new RegraRiscoMedio()
        };

        var regraAplicavel = regras.FirstOrDefault(regra => regra.Corresponde(texto));

        return regraAplicavel?.Nivel ?? Baixo;
    }
}
