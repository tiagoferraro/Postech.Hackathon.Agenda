using System;
using Postech.Hackathon.Agenda.Domain.Enums;

namespace Postech.Hackathon.Agenda.Domain.Entities;

public class Agendamento
{
    public Guid IdAgendamento { get; private set; }
    public Guid MedicoId { get; private set; }
    public Guid PacienteId { get; private set; }
    public DateTime DataHoraConsulta { get; private set; }
    public StatusConsultaEnum StatusConsulta { get; private set; }
    public string? JustificativaCancelamento { get; private set; }

    //dapper
    private Agendamento() { }
    public Agendamento(Guid medicoId, Guid pacienteId, DateTime dataHoraConsulta)
    {
        IdAgendamento = Guid.NewGuid();
        MedicoId = medicoId;
        PacienteId = pacienteId;
        DataHoraConsulta = dataHoraConsulta;
        StatusConsulta = StatusConsultaEnum.AguardandoAprovacaoMedico;        
    }
    public void RecusarConsulta(string justificativa)
    {
        if (StatusConsulta != StatusConsultaEnum.Aprovado)
        {
            throw new InvalidOperationException("A consulta não pode ser recusada, pois já foi aprovada.");
        }

        JustificativaCancelamento = justificativa;
        StatusConsulta = StatusConsultaEnum.Recusado;
    }
    public void AprovarConsulta()
    {
        if(DataHoraConsulta < DateTime.Now)
        {
            throw new InvalidOperationException("A consulta não pode ser aprovada, pois a data e hora já passaram.");
        }
        
        if (StatusConsulta != StatusConsultaEnum.AguardandoAprovacaoMedico)
        {
            throw new InvalidOperationException("A consulta não pode ser aprovada, pois não está aguardando aprovação do médico.");
        }

        StatusConsulta = StatusConsultaEnum.Aprovado;
    }
}
