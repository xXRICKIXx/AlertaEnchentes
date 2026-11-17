using AlertaEnchentes.Desktop.Models;

namespace AlertaEnchentes.Desktop.Services;

/// <summary>Ponto único de validação usado pelas telas (delega às próprias classes do domínio).</summary>
public static class Validador
{
    public static List<string> Validar(NovaOcorrencia ocorrencia) => ocorrencia.Validar();

    public static List<string> Validar(NovoAlerta alerta) => alerta.Validar();
}
