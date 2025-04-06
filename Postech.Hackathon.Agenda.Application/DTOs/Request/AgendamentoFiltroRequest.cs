namespace Postech.Hackathon.Agenda.Application.DTOs.Request;

public record AgendamentoFiltroRequest(
    Guid? MedicoId,
    Guid? PacienteId,
     DateTime? DataInicial,
     DateTime? DataFinal);
