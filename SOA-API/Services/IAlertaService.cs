using AlertaEnchentes.Api.Models;

namespace AlertaEnchentes.Api.Services;

public interface IAlertaService
{
    Task<List<Alerta>> ListarAsync();

    /// <exception cref="ArgumentException">Nível de risco inválido.</exception>
    Task<Alerta> CriarAsync(AlertaCreateDto dto);
}
