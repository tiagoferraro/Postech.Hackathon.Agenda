using Postech.Hackathon.Agenda.Application.DTOs;
using Postech.Hackathon.Agenda.Domain.Entities;

namespace Postech.Hackathon.Agenda.Application.Interfaces
{
    public interface IHorarioDisponivelService
    {
        Task<HorarioDisponivelDto> CriarHorarioDisponivelAsync(HorarioDisponivelDto dto);
        Task<IEnumerable<HorarioDisponivelDto>> ObterHorariosPorMedicoAsync(Guid medicoId);
    }
} 