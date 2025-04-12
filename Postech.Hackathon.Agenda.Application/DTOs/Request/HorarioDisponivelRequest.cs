using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postech.Hackathon.Agenda.Application.DTOs.Request
{
    public class HorarioDisponivelRequest
    {
        public Guid? HorarioDisponivelId { get; set; }
        public Guid MedicoId { get; set; }
        public int DiaSemana { get; set; }
        public TimeSpan Horas { get; set; }
    }
}
