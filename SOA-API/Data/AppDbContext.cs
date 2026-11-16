using AlertaEnchentes.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AlertaEnchentes.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Ocorrencia> Ocorrencias => Set<Ocorrencia>();
    public DbSet<Alerta> Alertas => Set<Alerta>();
}
