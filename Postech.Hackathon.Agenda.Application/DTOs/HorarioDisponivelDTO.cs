using Postech.Hackathon.Agenda.Domain.Entities;

namespace Postech.Hackathon.Agenda.Application.DTOs;

public class HorarioDisponivelDto
{
    public Guid IdHorarioDisponivel { get; set; }
    public Guid MedicoId { get; set; }
    public int DiaSemana { get; set; }
    public TimeSpan Horas { get; set; }
    public static HorarioDisponivelDto MapToDto(HorarioDisponivel horario)
    {
        return new HorarioDisponivelDto()
        {
            IdHorarioDisponivel = horario.IdHorarioDisponivel,
            MedicoId = horario.MedicoId,
            DiaSemana = horario.DiaSemana,
            Horas = horario.Horas
        };
    }
}


