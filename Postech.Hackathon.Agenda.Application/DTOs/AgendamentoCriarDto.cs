namespace Postech.Hackathon.Agenda.Application.DTOs;

    public record AgendamentoCriarDto(        
        Guid MedicoId,
        Guid PacienteId,
        DateTime DataHoraConsulta
    );


