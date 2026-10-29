using Abstracciones.Agentes;

namespace Flujo.Agentes;

internal static class AgenteMapeador
{
    public static AgenteDto ADto(Agente a) =>
        new(a.Id, a.Nombre, a.Correo, a.Telefono, a.CapacidadMaximaTickets, a.Activo, a.FechaIngreso);
}
