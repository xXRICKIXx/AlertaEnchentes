using AlertaEnchentes.Api.Data;
using AlertaEnchentes.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AlertaEnchentes.Api.Services;

public class AlertaService : IAlertaService
{
    private readonly AppDbContext _db;

    public AlertaService(AppDbContext db) => _db = db;

    public Task<List<Alerta>> ListarAsync() =>
        _db.Alertas.AsNoTracking().OrderByDescending(a => a.Data).ToListAsync();

    public async Task<Alerta> CriarAsync(AlertaCreateDto dto)
    {
        var nivel = NivelRisco.Normalizar(dto.NivelRisco)
            ?? throw new ArgumentException("Nível de risco inválido. Use Baixo, Médio ou Alto.");

        var alerta = new Alerta
        {
            Mensagem = dto.Mensagem.Trim(),
            NivelRisco = nivel,
            Data = DateTime.Now,
            Status = "Ativo"
        };

        _db.Alertas.Add(alerta);
        await _db.SaveChangesAsync();
        return alerta;
    }
}
