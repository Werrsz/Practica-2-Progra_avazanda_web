using Abstracciones.Comunes;

namespace Abstracciones.Agentes;

    public interface IAgenteDA   
{
    Task<ResultadoPaginado<Agente>> BuscarAsync(AgenteFiltro filtro, CancellationToken ct);
    Task<Agente?> ObtenerPorIdAsync(int id, CancellationToken ct);
    Task<Agente?> ObtenerPorCorreoAsync(string correo, CancellationToken ct);
    Task<Agente> AgregarAsync(Agente agente, CancellationToken ct);
    Task<bool> ActualizarAsync(Agente datos, CancellationToken ct);
    Task<bool> EliminarAsync(int id, CancellationToken ct);
}