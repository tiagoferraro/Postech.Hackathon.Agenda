using Postech.Hackathon.Agenda.Domain.Entities;

namespace Postech.Hackathon.Agenda.Infra.Interfaces
{
    public interface IAgendamentoRepository
    {
        Task<Agendamento> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Agendamento>> ObterPorMedicoAsync(Guid medicoId, DateTime dataInicial, DateTime datafinal);
        Task<IEnumerable<Agendamento>> ObterPorPacienteAsync(Guid pacienteId, DateTime dataInicial, DateTime datafinal);
        Task<bool> InserirAsync(Agendamento agendamento);
        Task<bool> AtualizarAsync(Agendamento agendamento);
        Task<bool> ExisteAgendamentoPorDataEMedicoAsync(DateTime dataHoraConsulta, Guid medicoId);
    }
} 