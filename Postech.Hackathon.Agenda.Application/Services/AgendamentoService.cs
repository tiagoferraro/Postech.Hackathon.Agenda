using Postech.Hackathon.Agenda.Application.DTOs.Request;
using Postech.Hackathon.Agenda.Application.DTOs.Response;
using Postech.Hackathon.Agenda.Application.Interfaces;
using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Infra.Interfaces;

namespace Postech.Hackathon.Agenda.Application.Services;

public class AgendamentoService(IAgendamentoRepository _agendamentoRepository) : IAgendamentoService
{    

    public async Task<AgendamentoResponse> CriarAsync(AgendamentoRequest dto)
    {
        var agendamentoExistente = await _agendamentoRepository.ExisteAgendamentoPorDataEMedicoAsync(dto.DataHoraConsulta,dto.MedicoId);
        if (agendamentoExistente)
        {
            throw new InvalidOperationException("Já existe um agendamento para este médico na data e hora informadas.");
        }

        var agendamento = new Agendamento(        
            medicoId: dto.MedicoId,
            pacienteId: dto.PacienteId,
            dataHoraConsulta: dto.DataHoraConsulta            
        );

        await _agendamentoRepository.InserirAsync(agendamento);
        return AgendamentoResponse.MapToDto(agendamento);
    }

    public async Task<AgendamentoResponse> ObterPorIdAsync(Guid agendamentoId)
    {
        return await _agendamentoRepository
            .ObterPorIdAsync(agendamentoId)
            .ContinueWith(t =>
            {           
                return AgendamentoResponse.MapToDto(t.Result);
            });
    }
    public async Task<IEnumerable<AgendamentoResponse>> ObterPorMedicoAsync(AgendamentoFiltroRequest agendamentoFiltroRequest)
    {
        if (!agendamentoFiltroRequest.MedicoId.HasValue)
        {
            throw new ArgumentException("O ID do médico é obrigatório.");
        }
     
        var listaAgendamentos = await _agendamentoRepository
            .ObterPorMedicoAsync(agendamentoFiltroRequest.MedicoId!.Value, agendamentoFiltroRequest.DataInicial, agendamentoFiltroRequest.DataFinal);
        return listaAgendamentos.Select(a => AgendamentoResponse.MapToDto(a));
    }
    public async Task<IEnumerable<AgendamentoResponse>> ObterPorPacienteAsync(AgendamentoFiltroRequest agendamentoFiltroRequest)
    {

        if (!agendamentoFiltroRequest.PacienteId.HasValue)
        {
            throw new ArgumentException("O ID do paciente é obrigatório.");
        }      

        var listaAgendamentos = await _agendamentoRepository
            .ObterPorPacienteAsync(agendamentoFiltroRequest.PacienteId!.Value, agendamentoFiltroRequest.DataInicial, agendamentoFiltroRequest.DataFinal);

        return listaAgendamentos.Select(a => AgendamentoResponse.MapToDto(a));
    }
    public async Task RecusarAsync(AgendamentoRecusaRequest agendamentoRecusaRequest)
    {
        var agendamento = await _agendamentoRepository.ObterPorIdAsync(agendamentoRecusaRequest.AgendamentoId);
        
        agendamento.RecusarConsulta(agendamentoRecusaRequest.StatusAgendamento, agendamentoRecusaRequest.Justificativa);

        await _agendamentoRepository.AtualizarAsync(agendamento);
    }
    public async Task AprovarAsync(AgendamentoAprovarRequest agendamentoAprovarRequest)
    {
        var agendamento = await _agendamentoRepository.ObterPorIdAsync(agendamentoAprovarRequest.AgendamentoId);
        
        agendamento.AprovarConsulta();

        await _agendamentoRepository.AtualizarAsync(agendamento);
    }

 

   
} 