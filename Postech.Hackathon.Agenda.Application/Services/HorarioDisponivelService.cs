using Microsoft.Extensions.Caching.Distributed;
using Postech.Hackathon.Agenda.Application.DTOs.Request;
using Postech.Hackathon.Agenda.Application.DTOs.Response;
using Postech.Hackathon.Agenda.Application.Interfaces;
using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Infra.Interfaces;
using System.Text.Json;

namespace Postech.Hackathon.Agenda.Application.Services;

public class HorarioDisponivelService : IHorarioDisponivelService
{
    private readonly IHorarioDisponivelRepository _horarioDisponivelRepository;
    private readonly IDistributedCache _cache;
    private const string CACHE_KEY_PREFIX = "horarios:medico:";

    public HorarioDisponivelService(
        IHorarioDisponivelRepository horarioDisponivelRepository,
        IDistributedCache cache)
    {
        _horarioDisponivelRepository = horarioDisponivelRepository;
        _cache = cache;
    }

    public async Task<HorarioDisponivelResponse> CriarHorarioDisponivelAsync(HorarioDisponivelRequest dto)
    {
        var horarioExistente = await _horarioDisponivelRepository.ExisteHorarioAsync(dto.MedicoId, dto.DiaSemana, dto.Horas);
        if (horarioExistente)
        {
            throw new InvalidOperationException("Já existe um horário cadastrado para este médico no mesmo dia e horário.");
        }

        var horarioDisponivel = new HorarioDisponivel(Guid.NewGuid(), dto.MedicoId, dto.DiaSemana, dto.Horas);
        await _horarioDisponivelRepository.InserirAsync(horarioDisponivel);

        // Invalida o cache para o médico
        await _cache.RemoveAsync($"{CACHE_KEY_PREFIX}{dto.MedicoId}");

        return HorarioDisponivelResponse.MapToDto(horarioDisponivel);
    }

    public async Task AlterarHorarioDisponivelAsync(HorarioDisponivelRequest dto)
    {
        if (dto.HorarioDisponivelId == null)
        {
            throw new InvalidOperationException("O ID do horário disponível é obrigatório.");
        }

        var horarioDisponivel = await _horarioDisponivelRepository.ObterPorIdAsync(dto.HorarioDisponivelId.Value)
            ?? throw new InvalidOperationException("Horário disponível não encontrado.");

        horarioDisponivel.AtualizarHorario(dto.DiaSemana, dto.Horas);
        await _horarioDisponivelRepository.AtualizarAsync(horarioDisponivel);

        // Invalida o cache para o médico
        await _cache.RemoveAsync($"{CACHE_KEY_PREFIX}{dto.MedicoId}");
    }

    public async Task<IEnumerable<HorarioDisponivelResponse>> ObterHorariosPorMedicoAsync(Guid medicoId)
    {
        var cacheKey = $"{CACHE_KEY_PREFIX}{medicoId}";
        var cachedData = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            var cachedHorarios = JsonSerializer.Deserialize<IEnumerable<HorarioDisponivel>>(cachedData);
            return cachedHorarios?.Select(h => HorarioDisponivelResponse.MapToDto(h)) ?? Enumerable.Empty<HorarioDisponivelResponse>();
        }

        var horarios = await _horarioDisponivelRepository.ObterPorMedicoAsync(medicoId);
        
        // Armazena no cache por 1 hora
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        };
        
        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(horarios),
            cacheOptions);

        return horarios.Select(h => HorarioDisponivelResponse.MapToDto(h));
    }
}