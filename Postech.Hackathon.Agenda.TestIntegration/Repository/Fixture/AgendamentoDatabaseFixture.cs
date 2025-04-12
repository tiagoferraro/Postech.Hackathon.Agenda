using Testcontainers.MsSql;

namespace Postech.Hackathon.Agenda.TestIntegration.Repository;

public class AgendamentoDatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _sqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPortBinding(1434, true)
        .Build();

    public string ConnectionString => _sqlContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _sqlContainer.StartAsync();
        await CriarTabelaAgendamento();
    }

    public async Task DisposeAsync()
    {
        await _sqlContainer.DisposeAsync();
    }

    private async Task CriarTabelaAgendamento()
    {
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(_sqlContainer.GetConnectionString());
        await connection.OpenAsync();

        var sql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Agendamento')
            BEGIN
                CREATE TABLE dbo.Agendamento (
                    AgendamentoId UNIQUEIDENTIFIER PRIMARY KEY,
                    MedicoId UNIQUEIDENTIFIER NOT NULL,
                    PacienteId UNIQUEIDENTIFIER NOT NULL,
                    DataHoraConsulta DATETIME NOT NULL,
                    StatusConsulta INT NOT NULL,
                    JustificativaCancelamento NVARCHAR(MAX),
                    DataCadastro DATETIME NOT NULL,
                    DataAlteracao DATETIME
                )
            END";

        using var command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }
}