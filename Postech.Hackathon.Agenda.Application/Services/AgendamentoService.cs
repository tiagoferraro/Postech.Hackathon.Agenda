using Postech.Hackathon.Agenda.Application.Dtos;
using Postech.Hackathon.Agenda.Application.DTOs;
using Postech.Hackathon.Agenda.Application.Interfaces;
using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Infra.Interfaces;

namespace Postech.Hackathon.Agenda.Application.Services;

public class AgendamentoService(IAgendamentoRepository _agendamentoRepository) : IAgendamentoService
{    

    public async Task<AgendamentoDto> CriarAsync(AgendamentoCriarDto dto)
    {
        var agendamento = new Agendamento(        
            medicoId: dto.MedicoId,
            pacienteId: dto.PacienteId,
            dataHoraConsulta: dto.DataHoraConsulta            
        );

        await _agendamentoRepository.InserirAsync(agendamento);
        return MapToDto(agendamento);
    }

    public async Task<AgendamentoDto> ObterPorIdAsync(Guid agendamentoId)
    {
        return await _agendamentoRepository
            .ObterPorIdAsync(agendamentoId)
            .ContinueWith(t =>
            {
                if (t.Result == null)
                {
                    throw new KeyNotFoundException($"Agendamento com ID {agendamentoId} não encontrado.");
                }
                return MapToDto(t.Result);
            });
    }

    private static AgendamentoDto MapToDto(Agendamento agendamento)
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