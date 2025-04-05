using Postech.Hackathon.Agenda.Domain.Entities;

namespace Postech.Hackathon.Agenda.Infra.Interfaces
{
    public interface IAgendamentoRepository
    {
        Task<Agendamento?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Agendamento>> ObterTodosAsync();
        Task<IEnumerable<Agendamento>> ObterPorMedicoAsync(Guid medicoId);
        Task<IEnumerable<Agendamento>> ObterPorPacienteAsync(Guid pacienteId);
        Task<bool> InserirAsync(Agendamento agendamento);
        Task<bool> AtualizarAsync(Agendamento agendamento);
        Task<bool> DeletarAsync(Guid id);
    }
} 