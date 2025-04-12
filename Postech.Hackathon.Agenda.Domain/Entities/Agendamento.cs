using Postech.Hackathon.Agenda.Domain.Enums;

namespace Postech.Hackathon.Agenda.Domain.Entities;

public class Agendamento
{
    public Guid AgendamentoId { get; private set; }
    public Guid MedicoId { get; private set; }
    public Guid PacienteId { get; private set; }
    public DateTime DataHoraConsulta { get; private set; }
    public StatusAgendamento StatusConsulta { get; private set; }
    public string? JustificativaCancelamento { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public DateTime? DataAlteracao { get; private set; }

    //dapper
    private Agendamento() { }
    public Agendamento(Guid medicoId, Guid pacienteId, DateTime dataHoraConsulta)
    {
        AgendamentoId = Guid.NewGuid();
        MedicoId = medicoId;
        PacienteId = pacienteId;
        DataHoraConsulta = dataHoraConsulta;
        StatusConsulta = StatusAgendamento.AguardandoAprovacaoMedico;        
        DataCadastro = DateTime.Now;
    }
    
    public void RecusarConsulta(StatusAgendamento statusAgendamento,string justificativa)
    {

        if (StatusConsulta == StatusAgendamento.Aprovado)
        {
            throw new InvalidOperationException("A consulta não pode ser recusada, pois já foi aprovada.");
        }
        var listaAgendamentosAceitos = new List<StatusAgendamento>
        {
            StatusAgendamento.RecusadoPaciente,
            StatusAgendamento.RecusadoMedico
        };
        if (!listaAgendamentosAceitos.Contains(statusAgendamento))
        {
            throw new InvalidOperationException("status Incorreto");
        }
        if (string.IsNullOrEmpty(justificativa))
        {
            throw new ArgumentException("A justificativa de cancelamento não pode ser nula ou vazia.");
        }

        JustificativaCancelamento = justificativa;
        StatusConsulta = statusAgendamento;
        DataAlteracao = DateTime.Now;
    }
    public void AprovarConsulta()
    {
        if(DataHoraConsulta < DateTime.Now)
        {
            throw new InvalidOperationException("A consulta não pode ser aprovada, pois a data e hora já passaram.");
        }
        
        if (StatusConsulta != StatusAgendamento.AguardandoAprovacaoMedico)
        {
            throw new InvalidOperationException("A consulta não pode ser aprovada, pois não está aguardando aprovação do médico.");
        }

        StatusConsulta = StatusAgendamento.Aprovado;
        DataAlteracao = DateTime.Now;
    }
}
