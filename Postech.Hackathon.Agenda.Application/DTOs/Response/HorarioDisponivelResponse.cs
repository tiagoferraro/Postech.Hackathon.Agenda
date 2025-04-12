using Postech.Hackathon.Agenda.Domain.Entities;

namespace Postech.Hackathon.Agenda.Application.DTOs.Response;

public class HorarioDisponivelResponse
{
    public Guid HorarioDisponivelId { get; set; }
    public Guid MedicoId { get; set; }
    public int DiaSemana { get; set; }
    public TimeSpan Horas { get; set; }
    public static HorarioDisponivelResponse MapToDto(HorarioDisponivel horario)
    {
        return new HorarioDisponivelResponse()
        {
            HorarioDisponivelId = horario.HorarioDisponivelId,
            MedicoId = horario.MedicoId,
            DiaSemana = horario.DiaSemana,
            Horas = horario.Horas
        };
    }
}


