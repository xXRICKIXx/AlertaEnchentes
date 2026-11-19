namespace AlertaEnchentes.Desktop.Models;

/// <summary>
/// Classe-base das regras de classificação de risco.
/// Cada regra especializada define seu nível e como identificar o risco.
/// </summary>
public abstract class RegraClassificacaoRisco
{
    public abstract string Nivel { get; }

    public abstract bool Corresponde(string descricao);
}

/// <summary>Identifica indícios de situações de risco alto.</summary>
public sealed class RegraRiscoAlto : RegraClassificacaoRisco
{
    private static readonly string[] Palavras =
    {
        "submers", "carro", "veículo", "veiculo", "interdit", "transbord", "enchente",
        "ilhad", "tomad", "correnteza", "idoso", "criança", "crianca", "energia"
    };

    public override string Nivel => NivelRisco.Alto;

    public override bool Corresponde(string descricao) =>
        Palavras.Any(descricao.Contains);
}

/// <summary>Identifica indícios de situações de risco médio.</summary>
public sealed class RegraRiscoMedio : RegraClassificacaoRisco
{
    private static readonly string[] Palavras =
    {
        "acúmulo", "acumulo", "bueiro", "entupid", "lent", "trânsito", "transito",
        "calçada", "calcada", "poça", "poca"
    };

    public override string Nivel => NivelRisco.Medio;

    public override bool Corresponde(string descricao) =>
        Palavras.Any(descricao.Contains);
}
