using Postech.Hackathon.Agenda.Application.Dtos;
using Postech.Hackathon.Agenda.Application.DTOs;

namespace Postech.Hackathon.Agenda.Application.Interfaces;

public interface IAgendamentoService
{
    Task<AgendamentoDto> ObterPorIdAsync(Guid agendamentoId);
    Task<AgendamentoDto> CriarAsync(AgendamentoCriarDto dto); 
} 