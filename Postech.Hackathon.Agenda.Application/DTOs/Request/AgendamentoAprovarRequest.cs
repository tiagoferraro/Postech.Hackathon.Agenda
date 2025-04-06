using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Postech.Hackathon.Agenda.Application.DTOs.Request;

    public record AgendamentoAprovarRequest(
        Guid AgendamentoId        
    );


