using Abstracciones.Comunes;

namespace Abstracciones.Agentes;

public interface IAgenteFlujo
{
    Task<ResultadoPaginado<AgenteDto>> ListarAsync(AgenteFiltro filtro, CancellationToken ct);
    Task<AgenteDto> ObtenerAsync(int id, CancellationToken ct);
    Task<AgenteDto> CrearAsync(AgenteCrearDto dto, CancellationToken ct);
    Task ActualizarAsync(int id, AgenteActualizarDto dto, CancellationToken ct);
    Task EliminarAsync(int id, CancellationToken ct);
}