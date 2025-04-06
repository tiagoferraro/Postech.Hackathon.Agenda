using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Infra.Interfaces;

namespace Postech.Hackathon.Agenda.Infra.Repositories;
public class HorarioDisponivelRepository(IOptions<DatabaseSettings> _databaseSettings) : IHorarioDisponivelRepository
{

    public async Task<HorarioDisponivel> ObterPorIdAsync(Guid id)
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = "SELECT * FROM dbo.HorarioDisponivel WHERE IdHorarioDisponivel = @Id";
        var horarioDisponivel = await connection.QueryFirstOrDefaultAsync<HorarioDisponivel>(sql, new { Id = id });
        return horarioDisponivel ?? throw new KeyNotFoundException($"Horário disponível com ID {id} não encontrado.");
    }

    public async Task<IEnumerable<HorarioDisponivel>> ObterTodosAsync()
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = "SELECT * FROM dbo.HorarioDisponivel";
        return await connection.QueryAsync<HorarioDisponivel>(sql);
    }

    public async Task<IEnumerable<HorarioDisponivel>> ObterPorMedicoAsync(Guid medicoId)
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = @"SELECT * FROM dbo.HorarioDisponivel 
                               WHERE MedicoId = @MedicoId 
                               ORDER BY DiaSemana, Horas";

        return await connection.QueryAsync<HorarioDisponivel>(sql, new { MedicoId = medicoId });
    }

    public async Task<IEnumerable<HorarioDisponivel>> ObterPorDiaSemanaAsync(int diaSemana)
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = "SELECT * FROM dbo.HorarioDisponivel WHERE DiaSemana = @DiaSemana";
        return await connection.QueryAsync<HorarioDisponivel>(sql, new { DiaSemana = diaSemana });
    }

    public async Task<IEnumerable<HorarioDisponivel>> ObterPorMedicoEDiaAsync(Guid medicoId, int diaSemana)
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = @"SELECT * FROM dbo.HorarioDisponivel 
                               WHERE MedicoId = @MedicoId 
                               AND DiaSemana = @DiaSemana
                               ORDER BY Horas";
        return await connection.QueryAsync<HorarioDisponivel>(sql, new { MedicoId = medicoId, DiaSemana = diaSemana });
    }

    public async Task<bool> InserirAsync(HorarioDisponivel horarioDisponivel)
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = @"INSERT INTO dbo.HorarioDisponivel 
                               (IdHorarioDisponivel, MedicoId, DiaSemana, Horas) 
                               VALUES (@IdHorarioDisponivel, @MedicoId, @DiaSemana, @Horas)";

        var rows = await connection.ExecuteAsync(sql, horarioDisponivel);
        return rows > 0;
    }

    public async Task<bool> AtualizarAsync(HorarioDisponivel horarioDisponivel)
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = @"UPDATE dbo.HorarioDisponivel 
                               SET MedicoId = @MedicoId, 
                                   DiaSemana = @DiaSemana, 
                                   Horas = @Horas 
                               WHERE IdHorarioDisponivel = @IdHorarioDisponivel";

        var rows = await connection.ExecuteAsync(sql, horarioDisponivel);
        return rows > 0;
    }

    public async Task<bool> DeletarAsync(Guid id)
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = "DELETE FROM dbo.HorarioDisponivel WHERE IdHorarioDisponivel = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public async Task<bool> ExisteHorarioAsync(Guid medicoId, int diaSemana, TimeSpan horas)
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = @"SELECT COUNT(1) 
                               FROM dbo.HorarioDisponivel 
                               WHERE MedicoId = @MedicoId 
                               AND DiaSemana = @DiaSemana 
                               AND Horas = @Horas";

        var count = await connection.ExecuteScalarAsync<int>(sql, new { MedicoId = medicoId, DiaSemana = diaSemana, Horas = horas });
        return count > 0;
    }

    public async Task<HorarioDisponivel> CriarAsync(HorarioDisponivel horario)
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = @"INSERT INTO dbo.HorarioDisponivel 
                               (IdHorarioDisponivel, MedicoId, DiaSemana, Horas) 
                               VALUES (@IdHorarioDisponivel, @MedicoId, @DiaSemana, @Horas)";

        await connection.ExecuteAsync(sql, horario);
        return horario;
    }
}
