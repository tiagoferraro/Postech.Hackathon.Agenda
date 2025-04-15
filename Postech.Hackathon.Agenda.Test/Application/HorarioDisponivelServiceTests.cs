using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Postech.Hackathon.Agenda.Application.DTOs.Request;
using Postech.Hackathon.Agenda.Application.DTOs.Response;
using Postech.Hackathon.Agenda.Application.Services;
using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Infra.Interfaces;
using Xunit;

namespace Postech.Hackathon.Agenda.Test.Application;

public class HorarioDisponivelServiceTests
{
    private readonly Mock<IHorarioDisponivelRepository> _mockHorarioDisponivelRepository;
    private readonly HorarioDisponivelService _service;
    private readonly Mock<IDistributedCache> _mockCache;

    public HorarioDisponivelServiceTests()
    {
        _mockHorarioDisponivelRepository = new Mock<IHorarioDisponivelRepository>();
        _mockCache = new Mock<IDistributedCache>();
        _service = new HorarioDisponivelService(_mockHorarioDisponivelRepository.Object,_mockCache.Object);
    }

    [Fact]
    public async Task CriarHorarioDisponivelAsync_QuandoDadosValidos_DeveCriarHorario()
    {
        // Arrange
        var request = new HorarioDisponivelRequest
        {
            MedicoId = Guid.NewGuid(),
            DiaSemana = 1,
            Horas = new TimeSpan(9, 0, 0)
        };

        _mockHorarioDisponivelRepository
            .Setup(x => x.ExisteHorarioAsync(request.MedicoId, request.DiaSemana, request.Horas))
            .ReturnsAsync(false);

        _mockHorarioDisponivelRepository
            .Setup(x => x.InserirAsync(It.IsAny<HorarioDisponivel>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CriarHorarioDisponivelAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.MedicoId, result.MedicoId);
        Assert.Equal(request.DiaSemana, result.DiaSemana);
        Assert.Equal(request.Horas, result.Horas);
    }

    [Fact]
    public async Task CriarHorarioDisponivelAsync_QuandoHorarioJaExiste_DeveLancarExcecao()
    {
        // Arrange
        var request = new HorarioDisponivelRequest
        {
            MedicoId = Guid.NewGuid(),
            DiaSemana = 1,
            Horas = new TimeSpan(9, 0, 0)
        };

        _mockHorarioDisponivelRepository
            .Setup(x => x.ExisteHorarioAsync(request.MedicoId, request.DiaSemana, request.Horas))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.CriarHorarioDisponivelAsync(request));
        Assert.Equal("Já existe um horário cadastrado para este médico no mesmo dia e horário.", exception.Message);
    }

    [Fact]
    public async Task AlterarHorarioDisponivelAsync_QuandoDadosValidos_DeveAtualizarHorario()
    {
        // Arrange
        var horarioId = Guid.NewGuid();
        var request = new HorarioDisponivelRequest
        {
            HorarioDisponivelId = horarioId,
            MedicoId = Guid.NewGuid(),
            DiaSemana = 2,
            Horas = new TimeSpan(14, 0, 0)
        };

        var horarioExistente = new HorarioDisponivel(horarioId, request.MedicoId, 1, new TimeSpan(9, 0, 0));

        _mockHorarioDisponivelRepository
            .Setup(x => x.ObterPorIdAsync(horarioId))
            .ReturnsAsync(horarioExistente);

        _mockHorarioDisponivelRepository
            .Setup(x => x.ExisteHorarioAsync(request.MedicoId, request.DiaSemana, request.Horas))
            .ReturnsAsync(false);

        _mockHorarioDisponivelRepository
            .Setup(x => x.AtualizarAsync(It.IsAny<HorarioDisponivel>()))
            .ReturnsAsync(true);

        // Act
        await _service.AlterarHorarioDisponivelAsync(request);

        // Assert
        Assert.Equal(request.DiaSemana, horarioExistente.DiaSemana);
        Assert.Equal(request.Horas, horarioExistente.Horas);
    }

    [Fact]
    public async Task AlterarHorarioDisponivelAsync_QuandoHorarioIdNaoInformado_DeveLancarExcecao()
    {
        // Arrange
        var request = new HorarioDisponivelRequest
        {
            MedicoId = Guid.NewGuid(),
            DiaSemana = 1,
            Horas = new TimeSpan(9, 0, 0)
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.AlterarHorarioDisponivelAsync(request));
        Assert.Equal("O ID do horário disponível é obrigatório.", exception.Message);
    }

    [Fact]
    public async Task AlterarHorarioDisponivelAsync_QuandoHorarioNaoEncontrado_DeveLancarExcecao()
    {
        // Arrange
        var horarioId = Guid.NewGuid();
        var request = new HorarioDisponivelRequest
        {
            HorarioDisponivelId = horarioId,
            MedicoId = Guid.NewGuid(),
            DiaSemana = 1,
            Horas = new TimeSpan(9, 0, 0)
        };

        _mockHorarioDisponivelRepository
            .Setup(x => x.ObterPorIdAsync(horarioId))
            .ReturnsAsync((HorarioDisponivel?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.AlterarHorarioDisponivelAsync(request));
        Assert.Equal("Horário disponível não encontrado.", exception.Message);
    }

    [Fact]
    public async Task ObterHorariosPorMedicoAsync_QuandoDadosValidos_DeveRetornarLista()
    {
        // Arrange
        var medicoId = Guid.NewGuid();
        var horarios = new List<HorarioDisponivel>
        {
            new(Guid.NewGuid(), medicoId, 1, new TimeSpan(9, 0, 0)),
            new(Guid.NewGuid(), medicoId, 2, new TimeSpan(14, 0, 0))
        };

        _mockHorarioDisponivelRepository
            .Setup(x => x.ObterPorMedicoAsync(medicoId))
            .ReturnsAsync(horarios);

        // Act
        var result = await _service.ObterHorariosPorMedicoAsync(medicoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
  

    [Fact]
    public async Task CriarHorarioDisponivelAsync_QuandoHorarioDuplicado_DeveLancarExcecaoComMensagemEspecifica()
    {
        // Arrange
        var medicoId = Guid.NewGuid();
        var diaSemana = 2; // Terça-feira
        var horas = new TimeSpan(14, 30, 0); // 14:30

        var request = new HorarioDisponivelRequest
        {
            MedicoId = medicoId,
            DiaSemana = diaSemana,
            Horas = horas
        };

        _mockHorarioDisponivelRepository
            .Setup(x => x.ExisteHorarioAsync(medicoId, diaSemana, horas))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.CriarHorarioDisponivelAsync(request));
        
        Assert.Equal("Já existe um horário cadastrado para este médico no mesmo dia e horário.", exception.Message);
        
        // Verifica se o repositório foi chamado corretamente
        _mockHorarioDisponivelRepository.Verify(
            x => x.ExisteHorarioAsync(medicoId, diaSemana, horas),
            Times.Once);
        
        // Verifica se o método de inserção não foi chamado
        _mockHorarioDisponivelRepository.Verify(
            x => x.InserirAsync(It.IsAny<HorarioDisponivel>()),
            Times.Never);
    }


 

}