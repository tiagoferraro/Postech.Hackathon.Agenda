using Postech.Hackathon.Agenda.Application.DTOs.Request;
using Postech.Hackathon.Agenda.Application.DTOs.Response;

namespace Postech.Hackathon.Agenda.Application.Interfaces;

public interface IAgendamentoService
{
    Task<AgendamentoResponse> ObterPorIdAsync(Guid agendamentoId);
    Task<AgendamentoResponse> CriarAsync(AgendamentoRequest dto);
    Task<IEnumerable<AgendamentoResponse>> ObterPorMedicoAsync(AgendamentoFiltroRequest agendamentoFiltroRequest);
    Task<IEnumerable<AgendamentoResponse>> ObterPorPacienteAsync(AgendamentoFiltroRequest agendamentoFiltroRequest);
    Task RecusarAsync(AgendamentoRecusaRequest agendamentoRecusaRequest);
    Task AprovarAsync(AgendamentoAprovarRequest agendamentoAprovarRequest);
} 