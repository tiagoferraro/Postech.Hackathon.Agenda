using Moq;
using Postech.Hackathon.Agenda.Application.DTOs.Request;
using Postech.Hackathon.Agenda.Application.DTOs.Response;
using Postech.Hackathon.Agenda.Application.Services;
using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Domain.Enums;
using Postech.Hackathon.Agenda.Infra.Interfaces;
using Xunit;

namespace Postech.Hackathon.Agenda.Tests.Unit.Application.Services;

public class AgendamentoServiceTests
{
    private readonly Mock<IAgendamentoRepository> _mockAgendamentoRepository;
    private readonly AgendamentoService _service;

    public AgendamentoServiceTests()
    {
        _mockAgendamentoRepository = new Mock<IAgendamentoRepository>();
        _service = new AgendamentoService(_mockAgendamentoRepository.Object);
    }

    [Fact]
    public async Task CriarAsync_QuandoDadosValidos_DeveCriarAgendamento()
    {
        // Arrange
        var request = new AgendamentoRequest
        {
            MedicoId = Guid.NewGuid(),
            PacienteId = Guid.NewGuid(),
            DataHoraConsulta = DateTime.Now.AddDays(1)
        };

        _mockAgendamentoRepository
            .Setup(x => x.ExisteAgendamentoPorDataEMedicoAsync(request.DataHoraConsulta, request.MedicoId))
            .ReturnsAsync(false);

        _mockAgendamentoRepository
            .Setup(x => x.InserirAsync(It.IsAny<Agendamento>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CriarAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.MedicoId, result.MedicoId);
        Assert.Equal(request.PacienteId, result.PacienteId);
        Assert.Equal(request.DataHoraConsulta, result.DataHoraConsulta);
        Assert.Equal(StatusAgendamento.AguardandoAprovacaoMedico, result.StatusConsulta);
    }

    [Fact]
    public async Task CriarAsync_QuandoAgendamentoJaExiste_DeveLancarExcecao()
    {
        // Arrange
        var request = new AgendamentoRequest
        {
            MedicoId = Guid.NewGuid(),
            PacienteId = Guid.NewGuid(),
            DataHoraConsulta = DateTime.Now.AddDays(1)
        };

        _mockAgendamentoRepository
            .Setup(x => x.ExisteAgendamentoPorDataEMedicoAsync(request.DataHoraConsulta, request.MedicoId))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarAsync(request));
        Assert.Equal("Já existe um agendamento para este médico na data e hora informadas.", exception.Message);
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoAgendamentoExiste_DeveRetornarAgendamento()
    {
        // Arrange
        var agendamentoId = Guid.NewGuid();
        var agendamento = new Agendamento(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(1));

        _mockAgendamentoRepository
            .Setup(x => x.ObterPorIdAsync(agendamentoId))
            .ReturnsAsync(agendamento);

        // Act
        var result = await _service.ObterPorIdAsync(agendamentoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(agendamento.AgendamentoId, result.AgendamentoId);
    }

    [Fact]
    public async Task ObterPorMedicoAsync_QuandoDadosValidos_DeveRetornarLista()
    {
        // Arrange
        var medicoId = Guid.NewGuid();
        var dataInicial = DateTime.Now;
        var dataFinal = DateTime.Now.AddDays(7);
        var agendamentos = new List<Agendamento>
        {
            new Agendamento(medicoId, Guid.NewGuid(), DateTime.Now.AddDays(1)),
            new Agendamento(medicoId, Guid.NewGuid(), DateTime.Now.AddDays(2))
        };

        _mockAgendamentoRepository
            .Setup(x => x.ObterPorMedicoAsync(medicoId, dataInicial, dataFinal))
            .ReturnsAsync(agendamentos);

        var request = new AgendamentoFiltroRequest(medicoId, null, dataInicial, dataFinal)
        {
            MedicoId = medicoId,
            DataInicial = dataInicial,
            DataFinal = dataFinal,
            PacienteId = null
        };

        // Act
        var result = await _service.ObterPorMedicoAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ObterPorMedicoAsync_QuandoMedicoIdNaoInformado_DeveLancarExcecao()
    {
        // Arrange
        var request = new AgendamentoFiltroRequest(null, null, DateTime.Now, DateTime.Now.AddDays(7));     

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.ObterPorMedicoAsync(request));
        Assert.Equal("O ID do médico é obrigatório.", exception.Message);
    }

    [Fact]
    public async Task RecusarAsync_QuandoDadosValidos_DeveRecusarAgendamento()
    {
        // Arrange
        var agendamentoId = Guid.NewGuid();
        var agendamento = new Agendamento(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(1));
        var request = new AgendamentoRecusaRequest(agendamentoId, "Médico indisponível")
        {
            AgendamentoId = agendamentoId,
            Justificativa = "Médico indisponível"
        };

        _mockAgendamentoRepository
            .Setup(x => x.ObterPorIdAsync(agendamentoId))
            .ReturnsAsync(agendamento);

        _mockAgendamentoRepository
            .Setup(x => x.AtualizarAsync(It.IsAny<Agendamento>()))
            .ReturnsAsync(true);

        // Act
        await _service.RecusarAsync(request);

        // Assert
        Assert.Equal(StatusAgendamento.Recusado, agendamento.StatusConsulta);
        Assert.Equal(request.Justificativa, agendamento.JustificativaCancelamento);
    }

    [Fact]
    public async Task AprovarAsync_QuandoDadosValidos_DeveAprovarAgendamento()
    {
        // Arrange
        var agendamentoId = Guid.NewGuid();
        var agendamento = new Agendamento(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(1));
        var request = new AgendamentoAprovarRequest(agendamentoId);

        _mockAgendamentoRepository
            .Setup(x => x.ObterPorIdAsync(agendamentoId))
            .ReturnsAsync(agendamento);

        _mockAgendamentoRepository
            .Setup(x => x.AtualizarAsync(It.IsAny<Agendamento>()))
            .ReturnsAsync(true);

        // Act
        await _service.AprovarAsync(request);

        // Assert
        Assert.Equal(StatusAgendamento.Aprovado, agendamento.StatusConsulta);
    }

    [Fact]
    public async Task ObterPorPacienteAsync_QuandoDadosValidos_DeveRetornarLista()
    {
        // Arrange
        var pacienteId = Guid.NewGuid();
        var dataInicial = DateTime.Now;
        var dataFinal = DateTime.Now.AddDays(7);
        var agendamentos = new List<Agendamento>
        {
            new Agendamento(Guid.NewGuid(), pacienteId, DateTime.Now.AddDays(1)),
            new Agendamento(Guid.NewGuid(), pacienteId, DateTime.Now.AddDays(2))
        };

        _mockAgendamentoRepository
            .Setup(x => x.ObterPorPacienteAsync(pacienteId, dataInicial, dataFinal))
            .ReturnsAsync(agendamentos);

        var request = new AgendamentoFiltroRequest(null, pacienteId, dataInicial, dataFinal);

        // Act
        var result = await _service.ObterPorPacienteAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, r => Assert.Equal(pacienteId, r.PacienteId));
    }

    [Fact]
    public async Task ObterPorPacienteAsync_QuandoPacienteIdNaoInformado_DeveLancarExcecao()
    {
        // Arrange
        var request = new AgendamentoFiltroRequest(null, null, DateTime.Now, DateTime.Now.AddDays(7));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.ObterPorPacienteAsync(request));
        Assert.Equal("O ID do paciente é obrigatório.", exception.Message);
    }
} 