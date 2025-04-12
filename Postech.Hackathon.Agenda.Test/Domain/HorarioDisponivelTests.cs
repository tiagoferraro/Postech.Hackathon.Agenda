using Postech.Hackathon.Agenda.Domain.Entities;

namespace Postech.Hackathon.Agenda.Tests.Unit.Domain.Entities;

public class HorarioDisponivelTests
{
    [Fact]
    public void Construtor_QuandoDadosValidos_DeveCriarHorarioDisponivel()
    {
        // Arrange
        var horarioDisponivelId = Guid.NewGuid();
        var medicoId = Guid.NewGuid();
        var diaSemana = 1; // Domingo
        var horas = new TimeSpan(9, 0, 0); // 09:00

        // Act
        var horarioDisponivel = new HorarioDisponivel(horarioDisponivelId, medicoId, diaSemana, horas);

        // Assert
        Assert.Equal(horarioDisponivelId, horarioDisponivel.HorarioDisponivelId);
        Assert.Equal(medicoId, horarioDisponivel.MedicoId);
        Assert.Equal(diaSemana, horarioDisponivel.DiaSemana);
        Assert.Equal(horas, horarioDisponivel.Horas);
    }

    [Fact]
    public void Construtor_QuandoDiaSemanaInvalido_DeveLancarExcecao()
    {
        // Arrange
        var horarioDisponivelId = Guid.NewGuid();
        var medicoId = Guid.NewGuid();
        var diaSemana = 0; // Dia inválido
        var horas = new TimeSpan(9, 0, 0);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            new HorarioDisponivel(horarioDisponivelId, medicoId, diaSemana, horas));
        Assert.Equal("O dia da semana deve estar entre 1 e 7.", exception.Message);
    }

    [Fact]
    public void Construtor_QuandoHorarioNaoMultiploDe30Minutos_DeveLancarExcecao()
    {
        // Arrange
        var horarioDisponivelId = Guid.NewGuid();
        var medicoId = Guid.NewGuid();
        var diaSemana = 1;
        var horas = new TimeSpan(9, 15, 0); // 09:15 (não é múltiplo de 30)

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            new HorarioDisponivel(horarioDisponivelId, medicoId, diaSemana, horas));
        Assert.Equal("O horário deve ser em intervalos de 30 minutos.", exception.Message);
    }

    [Fact]
    public void AtualizarHorario_QuandoDadosValidos_DeveAtualizarHorario()
    {
        // Arrange
        var horarioDisponivel = new HorarioDisponivel(
            Guid.NewGuid(),
            Guid.NewGuid(),
            1,
            new TimeSpan(9, 0, 0));

        var novoDiaSemana = 2;
        var novoHorario = new TimeSpan(14, 0, 0);

        // Act
        horarioDisponivel.AtualizarHorario(novoDiaSemana, novoHorario);

        // Assert
        Assert.Equal(novoDiaSemana, horarioDisponivel.DiaSemana);
        Assert.Equal(novoHorario, horarioDisponivel.Horas);
    }

    [Fact]
    public void AtualizarHorario_QuandoDiaSemanaInvalido_DeveLancarExcecao()
    {
        // Arrange
        var horarioDisponivel = new HorarioDisponivel(
            Guid.NewGuid(),
            Guid.NewGuid(),
            1,
            new TimeSpan(9, 0, 0));

        var novoDiaSemana = 8; // Dia inválido
        var novoHorario = new TimeSpan(14, 0, 0);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            horarioDisponivel.AtualizarHorario(novoDiaSemana, novoHorario));
        Assert.Equal("O dia da semana deve estar entre 1 e 7.", exception.Message);
    }

    [Fact]
    public void AtualizarHorario_QuandoHorarioNaoMultiploDe30Minutos_DeveLancarExcecao()
    {
        // Arrange
        var horarioDisponivel = new HorarioDisponivel(
            Guid.NewGuid(),
            Guid.NewGuid(),
            1,
            new TimeSpan(9, 0, 0));

        var novoDiaSemana = 2;
        var novoHorario = new TimeSpan(14, 15, 0); // 14:15 (não é múltiplo de 30)

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            horarioDisponivel.AtualizarHorario(novoDiaSemana, novoHorario));
        Assert.Equal("O horário deve ser em intervalos de 30 minutos.", exception.Message);
    }
} 