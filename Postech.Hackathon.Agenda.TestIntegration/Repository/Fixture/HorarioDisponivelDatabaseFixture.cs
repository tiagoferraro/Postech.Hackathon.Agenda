using Testcontainers.MsSql;

namespace Postech.Hackathon.Agenda.TestIntegration.Repository.Fixture;

public class HorarioDisponivelDatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _sqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPortBinding(1434, true)
        .Build();

    public string ConnectionString => _sqlContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _sqlContainer.StartAsync();
        await CriarTabelaHorarioDisponivel();
    }

    public async Task DisposeAsync()
    {
        await _sqlContainer.DisposeAsync();
    }

    private async Task CriarTabelaHorarioDisponivel()
    {
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(_sqlContainer.GetConnectionString());
        await connection.OpenAsync();

        var sql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'HorarioDisponivel')
            BEGIN
                CREATE TABLE dbo.HorarioDisponivel (
                    HorarioDisponivelId UNIQUEIDENTIFIER PRIMARY KEY,
                    MedicoId UNIQUEIDENTIFIER NOT NULL,
                    DiaSemana INT NOT NULL,
                    Horas TIME NOT NULL
                )
            END";

        using var command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }
} 