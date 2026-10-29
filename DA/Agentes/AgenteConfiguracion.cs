using Abstracciones.Agentes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Agentes;

public class AgenteConfiguracion : IEntityTypeConfiguration<Agente>
{
    public void Configure(EntityTypeBuilder<Agente> b)
    {
        b.ToTable("Agentes", t => t.HasCheckConstraint("CK_Agentes_Capacidad",
            "[CapacidadMaximaTickets] BETWEEN 1 AND 50"));
        b.HasKey(a => a.Id);

        b.Property(a => a.Nombre).IsRequired().HasMaxLength(100);
        b.Property(a => a.Correo).IsRequired().HasMaxLength(150);
        b.Property(a => a.Telefono).IsRequired().HasMaxLength(8);
        b.Property(a => a.FechaIngreso).HasDefaultValueSql("SYSUTCDATETIME()");
        // No usar HasDefaultValue(true) en Activo: EF lo pisaría al guardar false.

        b.HasIndex(a => a.Correo).IsUnique();
    }
}