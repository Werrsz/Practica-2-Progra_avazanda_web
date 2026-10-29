using Abstracciones.Agentes;
using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using DA.Comunes;
using DA.Contexto;
using Microsoft.EntityFrameworkCore;

namespace DA.Agentes;

public class AgenteDA : IAgenteDA
{
    private readonly SistemaSoporteContexto _ctx;
    public AgenteDA(SistemaSoporteContexto ctx) => _ctx = ctx;

    public async Task<ResultadoPaginado<Agente>> BuscarAsync(AgenteFiltro filtro, CancellationToken ct)
    {
        IQueryable<Agente> consulta = _ctx.Agentes.AsNoTracking();

        if (filtro.Activo.HasValue)
        {
            var activo = filtro.Activo.Value;
            consulta = consulta.Where(a => a.Activo == activo);
        }
        if (!string.IsNullOrWhiteSpace(filtro.Q))
        {
            var texto = filtro.Q.Trim();
            consulta = consulta.Where(a => a.Nombre.Contains(texto) || a.Correo.Contains(texto));
        }

        var total = await consulta.CountAsync(ct);
        var items = await consulta
            .OrderBy(a => a.Nombre).ThenBy(a => a.Id)
            .Skip((filtro.Pagina - 1) * filtro.Tamano)
            .Take(filtro.Tamano)
            .ToListAsync(ct);

        return new ResultadoPaginado<Agente>
        {
            Items = items,
            Total = total,
            Pagina = filtro.Pagina,
            Tamano = filtro.Tamano
        };
    }

    public Task<Agente?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
        _ctx.Agentes.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task<Agente?> ObtenerPorCorreoAsync(string correo, CancellationToken ct) =>
        _ctx.Agentes.AsNoTracking().FirstOrDefaultAsync(a => a.Correo == correo, ct);

    public async Task<Agente> AgregarAsync(Agente agente, CancellationToken ct)
    {
        _ctx.Agentes.Add(agente);
        try { await _ctx.SaveChangesAsync(ct); }
        catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
        {
            throw new ConflictoException($"Ya existe un agente con el correo '{agente.Correo}'.");
        }
        return agente;
    }

    public async Task<bool> ActualizarAsync(Agente datos, CancellationToken ct)
    {
        var existente = await _ctx.Agentes.FirstOrDefaultAsync(a => a.Id == datos.Id, ct);
        if (existente is null) return false;

        existente.Nombre = datos.Nombre;
        existente.Correo = datos.Correo;
        existente.Telefono = datos.Telefono;
        existente.CapacidadMaximaTickets = datos.CapacidadMaximaTickets;
        existente.Activo = datos.Activo;

        try { await _ctx.SaveChangesAsync(ct); }
        catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
        {
            throw new ConflictoException($"Ya existe otro agente con el correo '{datos.Correo}'.");
        }
        return true;
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken ct)
    {
        var existente = await _ctx.Agentes.FirstOrDefaultAsync(a => a.Id == id, ct);
        if (existente is null) return false;

        _ctx.Agentes.Remove(existente);
        try { await _ctx.SaveChangesAsync(ct); }
        catch (DbUpdateException ex) when (ErroresSql.EsLlaveForanea(ex))
        {
            throw new ConflictoException("El agente tiene tickets asociados y no se puede eliminar.");
        }
        return true;
    }
}