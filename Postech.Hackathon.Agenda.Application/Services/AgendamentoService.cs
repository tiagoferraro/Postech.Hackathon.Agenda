using Postech.Hackathon.Agenda.Application.Dtos;
using Postech.Hackathon.Agenda.Application.DTOs.Request;
using Postech.Hackathon.Agenda.Application.Interfaces;
using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Infra.Interfaces;

namespace Postech.Hackathon.Agenda.Application.Services;

public class AgendamentoService(IAgendamentoRepository _agendamentoRepository) : IAgendamentoService
{    

    public async Task<AgendamentoDto> CriarAsync(AgendamentoRequest dto)
    {
        var agendamento = new Agendamento(        
            medicoId: dto.MedicoId,
            pacienteId: dto.PacienteId,
            dataHoraConsulta: dto.DataHoraConsulta            
        );

        await _agendamentoRepository.InserirAsync(agendamento);
        return AgendamentoDto.MapToDto(agendamento);
    }

    public async Task<AgendamentoDto> ObterPorIdAsync(Guid agendamentoId)
    {
        return await _agendamentoRepository
            .ObterPorIdAsync(agendamentoId)
            .ContinueWith(t =>
            {           
                return AgendamentoDto.MapToDto(t.Result);
            });
    }
    public async Task<IEnumerable<AgendamentoDto>> ObterPorMedicoAsync(AgendamentoFiltroRequest agendamentoFiltroRequest)
    {
        if (!agendamentoFiltroRequest.MedicoId.HasValue)
        {
            throw new ArgumentException("O ID do médico é obrigatório.");
        }
        if (agendamentoFiltroRequest.DataInicial == null || agendamentoFiltroRequest.DataFinal == null)
        {
            throw new ArgumentException("As datas de início e fim são obrigatórias.");
        }
        var listaAgendamentos = await _agendamentoRepository
            .ObterPorMedicoAsync(agendamentoFiltroRequest.MedicoId!.Value, agendamentoFiltroRequest.DataInicial!.Value, agendamentoFiltroRequest.DataFinal!.Value);
        return listaAgendamentos.Select(a => AgendamentoDto.MapToDto(a));
    }
    public async Task<IEnumerable<AgendamentoDto>> ObterPorPacienteAsync(AgendamentoFiltroRequest agendamentoFiltroRequest)
    {

        if (!agendamentoFiltroRequest.PacienteId.HasValue)
        {
            throw new ArgumentException("O ID do paciente é obrigatório.");
        }
        if (agendamentoFiltroRequest.DataInicial == null || agendamentoFiltroRequest.DataFinal == null)
        {
            throw new ArgumentException("As datas de início e fim são obrigatórias.");
        }        

        var listaAgendamentos = await _agendamentoRepository
            .ObterPorPacienteAsync(agendamentoFiltroRequest.PacienteId!.Value, agendamentoFiltroRequest.DataInicial!.Value, agendamentoFiltroRequest.DataFinal!.Value);

        return listaAgendamentos.Select(a => AgendamentoDto.MapToDto(a));
    }
    public async Task RecusarAsync(AgendamentoRecusaRequest agendamentoRecusaRequest)
    {
        var agendamento = await _agendamentoRepository.ObterPorIdAsync(agendamentoRecusaRequest.AgendamentoId);
        
        agendamento.RecusarConsulta(agendamentoRecusaRequest.Justificativa);

        await _agendamentoRepository.AtualizarAsync(agendamento);
    }
    public async Task AprovarAsync(AgendamentoAprovarRequest agendamentoAprovarRequest)
    {
        var agendamento = await _agendamentoRepository.ObterPorIdAsync(agendamentoAprovarRequest.AgendamentoId);
        
        agendamento.AprovarConsulta();

        await _agendamentoRepository.AtualizarAsync(agendamento);
    }

 

   
} 