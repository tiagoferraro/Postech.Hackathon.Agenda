using Postech.Hackathon.Agenda.Domain.Entities;

namespace Postech.Hackathon.Agenda.Infra.Interfaces
{
    public interface IHorarioDisponivelRepository
    {
        Task<HorarioDisponivel> ObterPorIdAsync(Guid id);
        Task<IEnumerable<HorarioDisponivel>> ObterPorMedicoAsync(Guid medicoId);
        Task<bool> InserirAsync(HorarioDisponivel horarioDisponivel);
        Task<bool> AtualizarAsync(HorarioDisponivel horarioDisponivel);
        Task<bool> DeletarAsync(Guid id);
        Task<bool> ExisteHorarioAsync(Guid medicoId, int diaSemana, TimeSpan horas);
    }
} 