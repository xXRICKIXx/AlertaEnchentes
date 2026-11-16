using AlertaEnchentes.Api.Data;
using AlertaEnchentes.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AlertaEnchentes.Api.Services;

public class OcorrenciaService : IOcorrenciaService
{
    private readonly AppDbContext _db;
    private readonly IAlertaService _alertas;

    public OcorrenciaService(AppDbContext db, IAlertaService alertas)
    {
        _db = db;
        _alertas = alertas;
    }

    public Task<List<Ocorrencia>> ListarAsync() =>
        _db.Ocorrencias.AsNoTracking().OrderByDescending(o => o.Data).ToListAsync();

    public Task<Ocorrencia?> ObterAsync(int id) =>
        _db.Ocorrencias.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);

    public async Task<Ocorrencia> CriarAsync(OcorrenciaCreateDto dto)
    {
        var nivel = NivelRisco.Normalizar(dto.NivelRisco)
            ?? throw new ArgumentException("Nível de risco inválido. Use Baixo, Médio ou Alto.");

        var ocorrencia = new Ocorrencia
        {
            Local = dto.Local.Trim(),
            Tipo = dto.Tipo.Trim(),
            NivelRisco = nivel,
            Descricao = dto.Descricao.Trim(),
            Data = DateTime.Now,
            Status = StatusOcorrencia.Aberta
        };

        _db.Ocorrencias.Add(ocorrencia);
        await _db.SaveChangesAsync();

        // Regra de negócio: ocorrência de risco ALTO gera um alerta automaticamente.
        if (nivel == NivelRisco.Alto)
        {
            var mensagem = $"Risco alto em {ocorrencia.Local}: {ocorrencia.Descricao}";
            if (mensagem.Length > 300) mensagem = mensagem[..297] + "...";

            await _alertas.CriarAsync(new AlertaCreateDto
            {
                Mensagem = mensagem,
                NivelRisco = NivelRisco.Alto
            });
        }

        return ocorrencia;
    }

    public async Task<Ocorrencia?> AtualizarStatusAsync(int id, string status)
    {
        var novo = StatusOcorrencia.Normalizar(status)
            ?? throw new ArgumentException("Status inválido. Use Aberta, Em acompanhamento ou Resolvida.");

        var ocorrencia = await _db.Ocorrencias.FindAsync(id);
        if (ocorrencia is null) return null;

        ocorrencia.Status = novo;
        await _db.SaveChangesAsync();
        return ocorrencia;
    }
}
