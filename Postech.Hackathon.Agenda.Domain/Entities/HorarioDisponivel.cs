using System;

namespace Postech.Hackathon.Agenda.Domain.Entities;

public class HorarioDisponivel
{
    public Guid IdHorarioDisponivel { get; private set; }
    public Guid MedicoId { get; private set; }
    public int DiaSemana { get; private set; }
    public TimeSpan Horas { get; private set; }

    public HorarioDisponivel(Guid idHorarioDisponivel, Guid medicoId, int diaSemana, TimeSpan horas)
    {
        if (diaSemana < 1 || diaSemana > 7)
        {
            throw new ArgumentException("O dia da semana deve estar entre 1 e 7.", nameof(diaSemana));
        }

        if (horas.Minutes % 30 != 0)
        {
            throw new ArgumentException("O horário deve ser em intervalos de 30 minutos.", nameof(horas));
        }

        IdHorarioDisponivel = idHorarioDisponivel;
        MedicoId = medicoId;
        DiaSemana = diaSemana;
        Horas = horas;
    }
}
