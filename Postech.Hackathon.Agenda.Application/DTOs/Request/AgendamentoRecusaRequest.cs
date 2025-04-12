using Postech.Hackathon.Agenda.Domain.Enums;

namespace Postech.Hackathon.Agenda.Application.DTOs.Request;

public record AgendamentoRecusaRequest(StatusAgendamento StatusAgendamento ,Guid AgendamentoId,string Justificativa);



