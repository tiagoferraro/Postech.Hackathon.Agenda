namespace Postech.Hackathon.Agenda.Domain.Entities;

public class HorarioDisponivel
{
    public Guid HorarioDisponivelId { get; private set; }
    public Guid MedicoId { get; private set; }
    public int DiaSemana { get; private set; }
    public TimeSpan Horas { get; private set; }

    public HorarioDisponivel(Guid horarioDisponivelId, Guid medicoId, int diaSemana, TimeSpan horas)
    {      
        HorarioDisponivelId = horarioDisponivelId;
        MedicoId = medicoId;
        DiaSemana = diaSemana;
        Horas = horas;

        Validar();
    }
    public void AtualizarHorario(int diaSemana,TimeSpan horas)
    {
        DiaSemana = diaSemana;
        Horas = horas;
        Validar();
    }
    private void Validar()
    {
        if (DiaSemana < 1 || DiaSemana > 7)
        {
            throw new ArgumentException("O dia da semana deve estar entre 1 e 7.");
        }

        if (Horas.Minutes % 30 != 0)
        {
            throw new ArgumentException("O horário deve ser em intervalos de 30 minutos.");
        }
    }
}
