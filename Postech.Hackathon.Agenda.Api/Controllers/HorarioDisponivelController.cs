using Microsoft.AspNetCore.Mvc;
using Postech.Hackathon.Agenda.Api.Models;
using Postech.Hackathon.Agenda.Application.DTOs;
using Postech.Hackathon.Agenda.Application.Services;

namespace Postech.Hackathon.Agenda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HorarioDisponivelController(HorarioDisponivelService _horarioDisponivelService) : ControllerBase
{ 

    [HttpPost]
    [ProducesResponseType(typeof(HorarioDisponivelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<HorarioDisponivelDto>> CriarHorarioDisponivel([FromBody] HorarioDisponivelDto dto)
    {
        try
        {
            var horario = await _horarioDisponivelService.CriarHorarioDisponivelAsync(dto);
            return CreatedAtAction(nameof(ObterHorariosPorMedico), new { medicoId = horario.MedicoId }, horario);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut]
    [ProducesResponseType(typeof(HorarioDisponivelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<HorarioDisponivelDto>> AlterarHorarioDisponivel([FromBody] HorarioDisponivelDto dto)
    {
        try
        {
             await _horarioDisponivelService.AlterarHorarioDisponivelAsync(dto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("obterhorariospormedico/{medicoId}")]
    [ProducesResponseType(typeof(IEnumerable<HorarioDisponivelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<HorarioDisponivelDto>>> ObterHorariosPorMedico(Guid medicoId)
    {
        var horarios = await _horarioDisponivelService.ObterHorariosPorMedicoAsync(medicoId);
        return Ok(horarios);
    }

} 