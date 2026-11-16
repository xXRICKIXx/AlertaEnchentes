using AlertaEnchentes.Api.Models;

namespace AlertaEnchentes.Api.Services;

public interface IOcorrenciaService
{
    Task<List<Ocorrencia>> ListarAsync();
    Task<Ocorrencia?> ObterAsync(int id);

    /// <exception cref="ArgumentException">Nível de risco inválido.</exception>
    Task<Ocorrencia> CriarAsync(OcorrenciaCreateDto dto);

    /// <exception cref="ArgumentException">Status inválido.</exception>
    Task<Ocorrencia?> AtualizarStatusAsync(int id, string status);
}
