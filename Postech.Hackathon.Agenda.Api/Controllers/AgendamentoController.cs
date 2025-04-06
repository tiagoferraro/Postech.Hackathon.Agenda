using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Postech.Hackathon.Agenda.Api.Models;
using Postech.Hackathon.Agenda.Application.Dtos;
using Postech.Hackathon.Agenda.Application.DTOs.Request;
using Postech.Hackathon.Agenda.Application.Interfaces;


namespace Postech.Hackathon.Agenda.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgendamentoController(IAgendamentoService _agendamentoService) : ControllerBase
{
    [HttpGet("{agendamentoId}")]
    [Authorize]
    [ProducesResponseType(typeof(AgendamentoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgendamentoDto>> ObterPorId(Guid agendamentoId)
    {
        var agendamento = await _agendamentoService.ObterPorIdAsync(agendamentoId);
        return Ok(agendamento);
    }

    [HttpPost]
    [Authorize(Roles = "Medico,Administrador")]
    [ProducesResponseType(typeof(AgendamentoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgendamentoDto>> Criar([FromBody] AgendamentoRequest dto)
    {
        var agendamento = await _agendamentoService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = agendamento.IdAgendamento }, agendamento);
    }

    [HttpPost("oberpormedico")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<AgendamentoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<AgendamentoDto>>> ObterPorMedico([FromBody] AgendamentoFiltroRequest agendamentoFiltroRequest)
    {
        var agendamentos = await _agendamentoService.ObterPorMedicoAsync(agendamentoFiltroRequest);
        return Ok(agendamentos);
    }

    [HttpPost("obterporpaciente")]    
    [Authorize(Roles = "Paciente,Administrador")]
    [ProducesResponseType(typeof(IEnumerable<AgendamentoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<AgendamentoDto>>> ObterPorPaciente([FromBody] AgendamentoFiltroRequest agendamentoFiltroRequest)
    {
        var agendamentos = await _agendamentoService.ObterPorPacienteAsync(agendamentoFiltroRequest);
        return Ok(agendamentos);
    }

    [HttpPost("recusar")]
    [Authorize(Roles = "Medico,Administrador")]
    [ProducesResponseType(typeof(AgendamentoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgendamentoDto>> Recusar([FromBody] AgendamentoRecusaRequest agendamentoFiltroRequest)
    {
        await _agendamentoService.RecusarAsync(agendamentoFiltroRequest);
        return Ok();
    }

    [HttpPost("aprovar")]
    [Authorize(Roles = "Medico,Administrador")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AgendamentoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgendamentoDto>> Aprovar([FromBody] AgendamentoAprovarRequest agendamentoAprovarRequest)
    {
        await _agendamentoService.AprovarAsync(agendamentoAprovarRequest);
        return Ok();
    }
} 