using Postech.Hackathon.Agenda.Application.DTOs;
namespace Postech.Hackathon.Agenda.Application.Interfaces;

public interface IHorarioDisponivelService
{
    Task<HorarioDisponivelDto> CriarHorarioDisponivelAsync(HorarioDisponivelDto dto);
    Task AlterarHorarioDisponivelAsync(HorarioDisponivelDto dto);
    Task<IEnumerable<HorarioDisponivelDto>> ObterHorariosPorMedicoAsync(Guid medicoId);
}
