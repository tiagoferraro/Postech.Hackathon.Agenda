namespace Postech.Hackathon.Agenda.Application.DTOs.Request;

    public record AgendamentoRequest(        
        Guid MedicoId,
        Guid PacienteId,
        DateTime DataHoraConsulta
    );


