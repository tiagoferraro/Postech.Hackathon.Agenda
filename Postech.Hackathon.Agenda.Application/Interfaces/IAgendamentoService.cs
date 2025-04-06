using Postech.Hackathon.Agenda.Application.Dtos;
using Postech.Hackathon.Agenda.Application.DTOs.Request;

namespace Postech.Hackathon.Agenda.Application.Interfaces;

public interface IAgendamentoService
{
    Task<AgendamentoDto> ObterPorIdAsync(Guid agendamentoId);
    Task<AgendamentoDto> CriarAsync(AgendamentoRequest dto);
    Task<IEnumerable<AgendamentoDto>> ObterPorMedicoAsync(AgendamentoFiltroRequest agendamentoFiltroRequest);
    Task<IEnumerable<AgendamentoDto>> ObterPorPacienteAsync(AgendamentoFiltroRequest agendamentoFiltroRequest);
    Task RecusarAsync(AgendamentoRecusaRequest agendamentoRecusaRequest);
    Task AprovarAsync(AgendamentoAprovarRequest agendamentoAprovarRequest);
} 