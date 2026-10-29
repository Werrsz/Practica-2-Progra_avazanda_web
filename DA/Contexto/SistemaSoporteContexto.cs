using Abstracciones.Clientes;
using Abstracciones.Comentarios;
using Abstracciones.Etiquetas;
using Abstracciones.Tickets;
using Microsoft.EntityFrameworkCore;
using Abstracciones.Agentes;

namespace DA.Contexto;

public class SistemaSoporteContexto : DbContext
{
    public SistemaSoporteContexto(DbContextOptions<SistemaSoporteContexto> options) : base(options) { }
    public DbSet<Agente> Agentes => Set<Agente>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Comentario> Comentarios => Set<Comentario>();
    public DbSet<Etiqueta> Etiquetas => Set<Etiqueta>();
    public DbSet<TicketEtiqueta> TicketEtiquetas => Set<TicketEtiqueta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ****PROFESOR: **** Una clase de configuración por entidad, junto a su módulo (carpeta).
        // Esta línea las descubre todas: agregar un módulo no obliga a tocar este archivo.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SistemaSoporteContexto).Assembly);
    }
}
