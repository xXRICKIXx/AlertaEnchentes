using System.ComponentModel.DataAnnotations;

namespace AlertaEnchentes.Api.Models;

public class OcorrenciaCreateDto
{
    [Required(ErrorMessage = "O local é obrigatório.")]
    [StringLength(120)]
    public string Local { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo é obrigatório.")]
    [StringLength(50)]
    public string Tipo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nível de risco é obrigatório (Baixo, Médio ou Alto).")]
    public string NivelRisco { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(500, MinimumLength = 5, ErrorMessage = "A descrição deve ter entre 5 e 500 caracteres.")]
    public string Descricao { get; set; } = string.Empty;
}

public class StatusUpdateDto
{
    [Required(ErrorMessage = "O status é obrigatório.")]
    public string Status { get; set; } = string.Empty;
}

public class AlertaCreateDto
{
    [Required(ErrorMessage = "A mensagem é obrigatória.")]
    [StringLength(300, MinimumLength = 5)]
    public string Mensagem { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nível de risco é obrigatório (Baixo, Médio ou Alto).")]
    public string NivelRisco { get; set; } = string.Empty;
}
