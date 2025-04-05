using Postech.Hackathon.Agenda.Domain.Enums;

namespace Postech.Hackathon.Agenda.Application.Dtos;

public class AgendamentoDto
{
    public Guid IdAgendamento { get; set; }
    public Guid MedicoId { get; set; }
    public Guid PacienteId { get; set; }
    public DateTime DataHoraConsulta { get; set; }
    public StatusConsultaEnum StatusConsulta { get; set; }
    public string? JustificativaCancelamento { get; set; }
} 