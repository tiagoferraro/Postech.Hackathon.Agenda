using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Domain.Enums;

namespace Postech.Hackathon.Agenda.Application.DTOs.Response;

public class AgendamentoResponse
{
    public Guid AgendamentoId { get; set; }
    public Guid MedicoId { get; set; }
    public Guid PacienteId { get; set; }
    public DateTime DataHoraConsulta { get; set; }
    public StatusAgendamento StatusConsulta { get; set; }
    public string? JustificativaCancelamento { get; set; }

    public static AgendamentoResponse MapToDto(Agendamento agendamento)
    {
        return new AgendamentoResponse
        {
            AgendamentoId = agendamento.AgendamentoId,
            MedicoId = agendamento.MedicoId,
            PacienteId = agendamento.PacienteId,
            DataHoraConsulta = agendamento.DataHoraConsulta,
            StatusConsulta = agendamento.StatusConsulta,
            JustificativaCancelamento = agendamento.JustificativaCancelamento
        };
    }
} 
  