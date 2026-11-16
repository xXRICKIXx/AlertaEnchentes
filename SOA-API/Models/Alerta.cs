using System.ComponentModel.DataAnnotations;

namespace AlertaEnchentes.Api.Models;

public class Alerta
{
    public int Id { get; set; }

    [Required, MaxLength(300)]
    public string Mensagem { get; set; } = string.Empty;

    [Required, MaxLength(10)]
    public string NivelRisco { get; set; } = string.Empty;

    public DateTime Data { get; set; }

    [Required, MaxLength(30)]
    public string Status { get; set; } = "Ativo";
}
