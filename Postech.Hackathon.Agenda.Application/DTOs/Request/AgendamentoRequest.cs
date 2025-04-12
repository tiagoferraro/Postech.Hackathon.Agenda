namespace Postech.Hackathon.Agenda.Application.DTOs.Request;

public record AgendamentoRequest
{
    public required Guid MedicoId { get; init; }
    public required Guid PacienteId { get; init; }
    public required DateTime DataHoraConsulta { get; init; }
}


