using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Infra.Interfaces;

namespace Postech.Hackathon.Agenda.Infra.Repositories;

public class AgendamentoRepository(IOptions<DatabaseSettings> _databaseSettings) : IAgendamentoRepository
{

    public async Task<Agendamento> ObterPorIdAsync(Guid id)
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = "SELECT * FROM dbo.Agendamento WHERE IdAgendamento = @Id";
        
        var agendamento = await connection.QueryFirstOrDefaultAsync<Agendamento>(sql, new { Id = id });

        return agendamento ?? throw new KeyNotFoundException($"Agendamento com ID {id} não encontrado.");

    }


    public async Task<IEnumerable<Agendamento>> ObterPorMedicoAsync(Guid medicoId,DateTime dataInicial,DateTime datafinal)
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = "SELECT * FROM dbo.Agendamento WHERE MedicoId = @MedicoId";
        return await connection.QueryAsync<Agendamento>(sql, new { MedicoId = medicoId });
    }

    public async Task<IEnumerable<Agendamento>> ObterPorPacienteAsync(Guid pacienteId, DateTime dataInicial, DateTime datafinal)
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = "SELECT * FROM dbo.Agendamento WHERE PacienteId = @PacienteId";
        return await connection.QueryAsync<Agendamento>(sql, new { PacienteId = pacienteId });
    }

    public async Task<bool> InserirAsync(Agendamento agendamento)
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = @"INSERT INTO dbo.Agendamento 
                               (IdAgendamento, MedicoId, PacienteId, DataHoraConsulta, StatusConsulta, JustificativaCancelamento) 
                               VALUES (@IdAgendamento, @MedicoId, @PacienteId, @DataHoraConsulta, @StatusConsulta, @JustificativaCancelamento)";

        var rows = await connection.ExecuteAsync(sql, agendamento);
        return rows > 0;
    }

    public async Task<bool> AtualizarAsync(Agendamento agendamento)
    {
        using var connection = new SqlConnection(_databaseSettings.Value.ConnectionString);
        const string sql = @"UPDATE dbo.Agendamento 
                               SET MedicoId = @MedicoId, 
                                   PacienteId = @PacienteId, 
                                   DataHoraConsulta = @DataHoraConsulta, 
                                   StatusConsulta = @StatusConsulta, 
                                   JustificativaCancelamento = @JustificativaCancelamento 
                               WHERE IdAgendamento = @IdAgendamento";

        var rows = await connection.ExecuteAsync(sql, agendamento);
        return rows > 0;
    }

 
}
