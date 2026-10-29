using Abstracciones.Tickets;

namespace Abstracciones.Agentes;

public class Agente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public int CapacidadMaximaTickets { get; set; } = 50;
    public bool Activo { get; set; } = true;
    public DateTime FechaIngreso { get; set; }
    public List<Ticket> Tickets { get; set; } = new();
}