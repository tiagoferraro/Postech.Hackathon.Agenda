using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Postech.Hackathon.Agenda.Api.Models;
using Postech.Hackathon.Agenda.Application.Dtos;
using Postech.Hackathon.Agenda.Application.DTOs.Request;
using Postech.Hackathon.Agenda.Application.Interfaces;

namespace Postech.Hackathon.Agenda.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgendamentoController(IAgendamentoService _agendamentoService, ILogger<AgendamentoController> _logger) : ControllerBase
{
    
   

    [HttpGet("{agendamentoId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AgendamentoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgendamentoDto>> ObterPorId(Guid agendamentoId)
    {
        try
        {
            var agendamento = await _agendamentoService.ObterPorIdAsync(agendamentoId);
            return Ok(agendamento);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter agendamento {Id}", agendamentoId);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(AgendamentoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgendamentoDto>> Criar([FromBody] AgendamentoRequest dto)
    {
        try
        {
            var agendamento = await _agendamentoService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = agendamento.IdAgendamento }, agendamento);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar agendamento");
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }

    [HttpPost("oberpormedico")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<AgendamentoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<AgendamentoDto>>> ObterPorMedico([FromBody] AgendamentoFiltroRequest agendamentoFiltroRequest)
    {
        try
        {     
            var agendamentos = await _agendamentoService.ObterPorMedicoAsync(agendamentoFiltroRequest);
            return Ok(agendamentos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter agendamentos do médico {MedicoId}", agendamentoFiltroRequest.MedicoId);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }

    [HttpPost("obterporpaciente")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<AgendamentoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<AgendamentoDto>>> ObterPorPaciente([FromBody] AgendamentoFiltroRequest agendamentoFiltroRequest)
    {
        try
        {       
            var agendamentos = await _agendamentoService.ObterPorPacienteAsync(agendamentoFiltroRequest);
            return Ok(agendamentos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter agendamentos do paciente {PacienteId}", agendamentoFiltroRequest.PacienteId);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }
    [HttpPost("recusar")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AgendamentoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgendamentoDto>> Recusar([FromBody] AgendamentoRecusaRequest agendamentoFiltroRequest)
    {
        try
        {
             await _agendamentoService.RecusarAsync(agendamentoFiltroRequest);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recusar agendamento {Id}", agendamentoFiltroRequest.AgendamentoId);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }
    [HttpPost("aprovar")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AgendamentoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgendamentoDto>> Aprovar([FromBody] AgendamentoAprovarRequest agendamentoAprovarRequest)
    {
         try
        {
             await _agendamentoService.AprovarAsync(agendamentoAprovarRequest);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recusar agendamento {Id}", agendamentoAprovarRequest.AgendamentoId);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }   

} 