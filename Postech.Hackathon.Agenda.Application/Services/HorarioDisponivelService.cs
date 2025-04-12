using Postech.Hackathon.Agenda.Application.DTOs.Request;
using Postech.Hackathon.Agenda.Application.DTOs.Response;
using Postech.Hackathon.Agenda.Application.Interfaces;
using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Infra.Interfaces;


namespace Postech.Hackathon.Agenda.Application.Services;

public class HorarioDisponivelService(IHorarioDisponivelRepository _horarioDisponivelRepository) : IHorarioDisponivelService
{
   

    public async Task<HorarioDisponivelResponse> CriarHorarioDisponivelAsync(HorarioDisponivelRequest dto)
    {
        var horarioExistente = await _horarioDisponivelRepository.ExisteHorarioAsync(dto.MedicoId, dto.DiaSemana, dto.Horas);
        if (horarioExistente)
        {
            throw new InvalidOperationException("Já existe um horário cadastrado para este médico no mesmo dia e horário.");
        }

        var horarioDisponivel = new HorarioDisponivel(Guid.NewGuid(), dto.MedicoId, dto.DiaSemana, dto.Horas);
        await _horarioDisponivelRepository.InserirAsync(horarioDisponivel);
   

        return HorarioDisponivelResponse.MapToDto(horarioDisponivel);        
    }

    public async Task AlterarHorarioDisponivelAsync(HorarioDisponivelRequest dto)
    {
        if (dto.HorarioDisponivelId == null)
        {
            throw new InvalidOperationException("O ID do horário disponível é obrigatório.");           
        }

        var horarioDisponivel = await _horarioDisponivelRepository.ObterPorIdAsync(dto.HorarioDisponivelId.Value)
            ?? throw new InvalidOperationException("Horário disponível não encontrado.");           

        horarioDisponivel.AtualizarHorario(dto.DiaSemana, dto.Horas);
        await _horarioDisponivelRepository.AtualizarAsync(horarioDisponivel);
      

    }

    public async Task<IEnumerable<HorarioDisponivelResponse>> ObterHorariosPorMedicoAsync(Guid medicoId)
    {
        var listaHorairos = await _horarioDisponivelRepository
            .ObterPorMedicoAsync(medicoId);
        return listaHorairos.Select(h => HorarioDisponivelResponse.MapToDto(h));
    }
   
}