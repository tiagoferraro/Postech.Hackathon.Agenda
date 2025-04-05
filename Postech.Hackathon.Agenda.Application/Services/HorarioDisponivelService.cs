using Postech.Hackathon.Agenda.Application.DTOs;
using Postech.Hackathon.Agenda.Application.Interfaces;
using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Infra.Interfaces;


namespace Postech.Hackathon.Agenda.Application.Services;

public class HorarioDisponivelService(IHorarioDisponivelRepository _horarioDisponivelRepository) : IHorarioDisponivelService
{
    public async Task<HorarioDisponivelDto> CriarHorarioDisponivelAsync(HorarioDisponivelDto dto)
    {
        var horarioExistente = await _horarioDisponivelRepository.ExisteHorarioAsync(dto.MedicoId, dto.DiaSemana, dto.Horas);
        if (horarioExistente)
        {
            throw new InvalidOperationException("Já existe um horário cadastrado para este médico no mesmo dia e horário.");
        }

        var horario = new HorarioDisponivel(Guid.NewGuid(), dto.MedicoId, dto.DiaSemana, dto.Horas);
        var operacaoRealizada = await _horarioDisponivelRepository.InserirAsync(horario);

        if (!operacaoRealizada)
        {
            throw new InvalidOperationException("Erro ao criar horário disponível.");
        }

        return new HorarioDisponivelDto
        (
            IdHorarioDisponivel: horario.IdHorarioDisponivel,
            MedicoId: horario.MedicoId,
            DiaSemana: horario.DiaSemana,
            Horas: horario.Horas
        );
    }

    public async Task<IEnumerable<HorarioDisponivelDto>> ObterHorariosPorMedicoAsync(Guid medicoId)
    {
        var listaHorairos = await _horarioDisponivelRepository
            .ObterPorMedicoAsync(medicoId);
        return listaHorairos.Select(h => MapToDto(h));
    }
    private static HorarioDisponivelDto MapToDto(HorarioDisponivel horario)
    {
        return new HorarioDisponivelDto
        (
            IdHorarioDisponivel: horario.IdHorarioDisponivel,
            MedicoId: horario.MedicoId,
            DiaSemana: horario.DiaSemana,
            Horas: horario.Horas
        );
    }
}