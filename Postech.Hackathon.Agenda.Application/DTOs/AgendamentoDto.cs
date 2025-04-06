using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Domain.Enums;

namespace Postech.Hackathon.Agenda.Application.Dtos;

public class AgendamentoDto
{
    public Guid IdAgendamento { get; set; }
    public Guid MedicoId { get; set; }
    public Guid PacienteId { get; set; }
    public DateTime DataHoraConsulta { get; set; }
    public StatusAgendamento StatusConsulta { get; set; }
    public string? JustificativaCancelamento { get; set; }

    public static AgendamentoDto MapToDto(Agendamento agendamento)
    {
        return new AgendamentoDto
        {
            IdAgendamento = agendamento.IdAgendamento,
            MedicoId = agendamento.MedicoId,
            PacienteId = agendamento.PacienteId,
            DataHoraConsulta = agendamento.DataHoraConsulta,
            StatusConsulta = agendamento.StatusConsulta,
            JustificativaCancelamento = agendamento.JustificativaCancelamento
        };
    }
} 
  