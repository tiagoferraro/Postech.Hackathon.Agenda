using Microsoft.Extensions.Options;
using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Domain.Enums;
using Postech.Hackathon.Agenda.Infra.Repositories;
using Postech.Hackathon.Agenda.TestIntegration.Repository.Fixture;
using Xunit;

namespace Postech.Hackathon.Agenda.TestIntegration.Repository
{
    public class AgendamentoRepositoryTest : IClassFixture<AgendamentoDatabaseFixture>
    {
        private readonly AgendamentoRepository _repository;
        private readonly DatabaseSettings _databaseSettings;

        public AgendamentoRepositoryTest(AgendamentoDatabaseFixture fixture)
        {
            _databaseSettings = new DatabaseSettings
            {
                ConnectionString = fixture.ConnectionString
            };

            _repository = new AgendamentoRepository(Options.Create(_databaseSettings));
        }

        [Fact]
        public async Task Deve_Inserir_E_Obter_Agendamento_Com_Sucesso()
        {
            // Arrange
            var agendamento = new Agendamento(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);

            // Act
            var resultadoInserir = await _repository.InserirAsync(agendamento);
            var agendamentoSalvo = await _repository.ObterPorIdAsync(agendamento.AgendamentoId);

            // Assert
            Assert.True(resultadoInserir);
            Assert.NotNull(agendamentoSalvo);
            Assert.Equal(agendamento.AgendamentoId, agendamentoSalvo.AgendamentoId);
            Assert.Equal(agendamento.MedicoId, agendamentoSalvo.MedicoId);
            Assert.Equal(agendamento.PacienteId, agendamentoSalvo.PacienteId);
        }

        [Fact]
        public async Task Deve_Atualizar_Agendamento_Com_Sucesso()
        {
            // Arrange
            var agendamento = new Agendamento(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);
            await _repository.InserirAsync(agendamento);

            // Act
            agendamento.RecusarConsulta(StatusAgendamento.RecusadoMedico, "Paciente desistiu");
            var resultadoAtualizar = await _repository.AtualizarAsync(agendamento);
            var agendamentoAtualizado = await _repository.ObterPorIdAsync(agendamento.AgendamentoId);

            // Assert
            Assert.True(resultadoAtualizar);
            Assert.Equal(StatusAgendamento.RecusadoMedico, agendamentoAtualizado.StatusConsulta);
            Assert.Equal("Paciente desistiu", agendamentoAtualizado.JustificativaCancelamento);
        }

        [Fact]
        public async Task Deve_Obter_Agendamentos_Por_Medico_Com_Sucesso()
        {
            // Arrange
            var medicoId = Guid.NewGuid();
            var dataInicial = DateTime.Now;
            var dataFinal = dataInicial.AddDays(7);

            var agendamento1 = new Agendamento(medicoId, Guid.NewGuid(), dataInicial.AddDays(1));
            var agendamento2 = new Agendamento(medicoId, Guid.NewGuid(), dataInicial.AddDays(2));

            await _repository.InserirAsync(agendamento1);
            await _repository.InserirAsync(agendamento2);

            // Act
            var agendamentos = await _repository.ObterPorMedicoAsync(medicoId, dataInicial, dataFinal);

            // Assert
            Assert.NotNull(agendamentos);
            Assert.Equal(2, agendamentos.Count());
            Assert.All(agendamentos, a => Assert.Equal(medicoId, a.MedicoId));
        }

        [Fact]
        public async Task Deve_Obter_Agendamentos_Por_Paciente_Com_Sucesso()
        {
            // Arrange
            var pacienteId = Guid.NewGuid();
            var dataInicial = DateTime.Now;
            var dataFinal = dataInicial.AddDays(7);

            var agendamento1 = new Agendamento(Guid.NewGuid(), pacienteId, dataInicial.AddDays(1));
            var agendamento2 = new Agendamento(Guid.NewGuid(), pacienteId, dataInicial.AddDays(2));

            await _repository.InserirAsync(agendamento1);
            await _repository.InserirAsync(agendamento2);

            // Act
            var agendamentos = await _repository.ObterPorPacienteAsync(pacienteId, dataInicial, dataFinal);

            // Assert
            Assert.NotNull(agendamentos);
            Assert.Equal(2, agendamentos.Count());
            Assert.All(agendamentos, a => Assert.Equal(pacienteId, a.PacienteId));
        }

        [Fact]
        public async Task Deve_Verificar_Existencia_De_Agendamento_Por_Data_E_Medico()
        {
            // Arrange
            var medicoId = Guid.NewGuid();
            var dataHoraConsulta = DateTime.Now;
            var agendamento = new Agendamento(medicoId, Guid.NewGuid(), dataHoraConsulta);
            await _repository.InserirAsync(agendamento);

            // Act
            var existe = await _repository.ExisteAgendamentoPorDataEMedicoAsync(dataHoraConsulta, medicoId);

            // Assert
            Assert.True(existe);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Ao_Obter_Agendamento_Inexistente()
        {
            // Arrange
            var idInexistente = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.ObterPorIdAsync(idInexistente));
        }
    }
}
