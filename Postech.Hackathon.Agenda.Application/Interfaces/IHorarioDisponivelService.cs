using Postech.Hackathon.Agenda.Application.DTOs.Request;
using Postech.Hackathon.Agenda.Application.DTOs.Response;
namespace Postech.Hackathon.Agenda.Application.Interfaces;

public interface IHorarioDisponivelService
{
    Task<HorarioDisponivelResponse> CriarHorarioDisponivelAsync(HorarioDisponivelRequest dto);
    Task AlterarHorarioDisponivelAsync(HorarioDisponivelRequest dto);
    Task<IEnumerable<HorarioDisponivelResponse>> ObterHorariosPorMedicoAsync(Guid medicoId);
}
