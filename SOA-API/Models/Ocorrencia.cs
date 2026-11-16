using System.ComponentModel.DataAnnotations;

namespace AlertaEnchentes.Api.Models;

public class Ocorrencia
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Local { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Tipo { get; set; } = string.Empty;

    [Required, MaxLength(10)]
    public string NivelRisco { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string Descricao { get; set; } = string.Empty;

    public DateTime Data { get; set; }

    [Required, MaxLength(30)]
    public string Status { get; set; } = StatusOcorrencia.Aberta;
}
