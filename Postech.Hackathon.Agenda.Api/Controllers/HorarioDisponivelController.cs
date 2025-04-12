using Microsoft.AspNetCore.Mvc;
using Postech.Hackathon.Agenda.Api.Models;
using Postech.Hackathon.Agenda.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Postech.Hackathon.Agenda.Application.DTOs.Response;
using Postech.Hackathon.Agenda.Application.DTOs.Request;

namespace Postech.Hackathon.Agenda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HorarioDisponivelController(IHorarioDisponivelService _horarioDisponivelService) : ControllerBase
{ 

    [HttpPost]
    [Authorize(Roles = "Medico,Administrador")]
    [ProducesResponseType(typeof(HorarioDisponivelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<HorarioDisponivelResponse>> CriarHorarioDisponivel([FromBody] HorarioDisponivelRequest dto)
    {
        var horario = await _horarioDisponivelService.CriarHorarioDisponivelAsync(dto);
        return CreatedAtAction(nameof(ObterHorariosPorMedico), new { medicoId = horario.MedicoId }, horario);
    }

    [HttpPut]
    [Authorize(Roles = "Medico,Administrador")]
    [ProducesResponseType(typeof(HorarioDisponivelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<HorarioDisponivelResponse>> AlterarHorarioDisponivel([FromBody] HorarioDisponivelRequest dto)
    {
        await _horarioDisponivelService.AlterarHorarioDisponivelAsync(dto);
        return NoContent();
    }

    [HttpGet("obterhorariospormedico/{medicoId}")]
    [Authorize(Roles = "Medico,Administrador")]
    [ProducesResponseType(typeof(IEnumerable<HorarioDisponivelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<HorarioDisponivelResponse>>> ObterHorariosPorMedico(Guid medicoId)
    {
        var horarios = await _horarioDisponivelService.ObterHorariosPorMedicoAsync(medicoId);
        return Ok(horarios);
    }
} 