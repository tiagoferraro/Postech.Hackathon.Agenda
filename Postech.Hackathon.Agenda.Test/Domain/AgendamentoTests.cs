using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Domain.Enums;

namespace Postech.Hackathon.Agenda.Test.Domain;

public class AgendamentoTests
{
    [Fact]
    public void Construtor_QuandoDadosValidos_DeveCriarAgendamento()
    {
        // Arrange
        var medicoId = Guid.NewGuid();
        var pacienteId = Guid.NewGuid();
        var dataHoraConsulta = DateTime.Now.AddDays(1);

        // Act
        var agendamento = new Agendamento(medicoId, pacienteId, dataHoraConsulta);

        // Assert
        Assert.NotEqual(Guid.Empty, agendamento.AgendamentoId);
        Assert.Equal(medicoId, agendamento.MedicoId);
        Assert.Equal(pacienteId, agendamento.PacienteId);
        Assert.Equal(dataHoraConsulta, agendamento.DataHoraConsulta);
        Assert.Equal(StatusAgendamento.AguardandoAprovacaoMedico, agendamento.StatusConsulta);
        Assert.Null(agendamento.JustificativaCancelamento);        
        Assert.Null(agendamento.DataAlteracao);
    }

    [Fact]
    public void RecusarConsulta_QuandoStatusAguardandoAprovacao_DeveRecusarConsulta()
    {
        // Arrange
        var agendamento = new Agendamento(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(1));
        var justificativa = "Médico indisponível";

        // Act
        agendamento.RecusarConsulta(StatusAgendamento.RecusadoMedico,justificativa);

        // Assert
        Assert.Equal(StatusAgendamento.RecusadoMedico, agendamento.StatusConsulta);
        Assert.Equal(justificativa, agendamento.JustificativaCancelamento);
        Assert.NotNull(agendamento.DataAlteracao);
    }

    [Fact]
    public void RecusarConsulta_QuandoStatusAprovado_DeveLancarExcecao()
    {
        // Arrange
        var agendamento = new Agendamento(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(1));
        agendamento.AprovarConsulta();
        var justificativa = "Médico indisponível";

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => agendamento.RecusarConsulta(StatusAgendamento.RecusadoMedico,justificativa));
        Assert.Equal("A consulta não pode ser recusada, pois já foi aprovada.", exception.Message);
    }

    [Fact]
    public void AprovarConsulta_QuandoStatusAguardandoAprovacao_DeveAprovarConsulta()
    {
        // Arrange
        var agendamento = new Agendamento(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(1));

        // Act
        agendamento.AprovarConsulta();

        // Assert
        Assert.Equal(StatusAgendamento.Aprovado, agendamento.StatusConsulta);
        Assert.NotNull(agendamento.DataAlteracao);
    }

    [Fact]
    public void AprovarConsulta_QuandoDataPassada_DeveLancarExcecao()
    {
        // Arrange
        var agendamento = new Agendamento(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(-1));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => agendamento.AprovarConsulta());
        Assert.Equal("A consulta não pode ser aprovada, pois a data e hora já passaram.", exception.Message);
    }

    [Fact]
    public void AprovarConsulta_QuandoStatusNaoAguardandoAprovacao_DeveLancarExcecao()
    {
        // Arrange
        var agendamento = new Agendamento(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(1));
        agendamento.AprovarConsulta();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => agendamento.AprovarConsulta());
        Assert.Equal("A consulta não pode ser aprovada, pois não está aguardando aprovação do médico.", exception.Message);
    }
    
} 