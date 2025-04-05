using System;

namespace Postech.Hackathon.Agenda.Application.DTOs;

public record HorarioDisponivelDto(
    Guid IdHorarioDisponivel,
    Guid MedicoId,
    int DiaSemana,
    TimeSpan Horas
); 