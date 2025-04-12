using Microsoft.Extensions.Options;
using Postech.Hackathon.Agenda.Domain.Entities;
using Postech.Hackathon.Agenda.Infra.Repositories;
using Postech.Hackathon.Agenda.TestIntegration.Repository.Fixture;
using Xunit;

namespace Postech.Hackathon.Agenda.TestIntegration.Repository
{
    public class HorarioDisponivelRepositoryTest : IClassFixture<HorarioDisponivelDatabaseFixture>
    {
        private readonly HorarioDisponivelRepository _repository;
        private readonly DatabaseSettings _databaseSettings;

        public HorarioDisponivelRepositoryTest(HorarioDisponivelDatabaseFixture fixture)
        {
            _databaseSettings = new DatabaseSettings
            {
                ConnectionString = fixture.ConnectionString
            };

            _repository = new HorarioDisponivelRepository(Options.Create(_databaseSettings));
        }

        [Fact]
        public async Task Deve_Inserir_E_Obter_HorarioDisponivel_Com_Sucesso()
        {
            // Arrange
            var horarioDisponivel = new HorarioDisponivel(
                Guid.NewGuid(),
                Guid.NewGuid(),
                1, // Segunda-feira
                TimeSpan.FromHours(9) // 09:00
            );

            // Act
            var resultadoInserir = await _repository.InserirAsync(horarioDisponivel);
            var horarioSalvo = await _repository.ObterPorIdAsync(horarioDisponivel.HorarioDisponivelId);

            // Assert
            Assert.True(resultadoInserir);
            Assert.NotNull(horarioSalvo);
            Assert.Equal(horarioDisponivel.HorarioDisponivelId, horarioSalvo.HorarioDisponivelId);
            Assert.Equal(horarioDisponivel.MedicoId, horarioSalvo.MedicoId);
            Assert.Equal(horarioDisponivel.DiaSemana, horarioSalvo.DiaSemana);
            Assert.Equal(horarioDisponivel.Horas, horarioSalvo.Horas);
        }

        [Fact]
        public async Task Deve_Obter_Horarios_Por_Medico_Com_Sucesso()
        {
            // Arrange
            var medicoId = Guid.NewGuid();
            var horario1 = new HorarioDisponivel(
                Guid.NewGuid(),
                medicoId,
                1, // Segunda-feira
                TimeSpan.FromHours(9) // 09:00
            );
            var horario2 = new HorarioDisponivel(
                Guid.NewGuid(),
                medicoId,
                2, // Terça-feira
                TimeSpan.FromHours(14) // 14:00
            );

            await _repository.InserirAsync(horario1);
            await _repository.InserirAsync(horario2);

            // Act
            var horarios = await _repository.ObterPorMedicoAsync(medicoId);

            // Assert
            Assert.NotNull(horarios);
            Assert.Equal(2, horarios.Count());
            Assert.Contains(horarios, h => h.HorarioDisponivelId == horario1.HorarioDisponivelId);
            Assert.Contains(horarios, h => h.HorarioDisponivelId == horario2.HorarioDisponivelId);
        }

        [Fact]
        public async Task Deve_Obter_Horarios_Por_Medico_E_Dia_Com_Sucesso()
        {
            // Arrange
            var medicoId = Guid.NewGuid();
            var diaSemana = 1; // Segunda-feira
            var horario1 = new HorarioDisponivel(
                Guid.NewGuid(),
                medicoId,
                diaSemana,
                TimeSpan.FromHours(9) // 09:00
            );
            var horario2 = new HorarioDisponivel(
                Guid.NewGuid(),
                medicoId,
                diaSemana,
                TimeSpan.FromHours(14) // 14:00
            );

            await _repository.InserirAsync(horario1);
            await _repository.InserirAsync(horario2);

            // Act
            var horarios = await _repository.ObterPorMedicoEDiaAsync(medicoId, diaSemana);

            // Assert
            Assert.NotNull(horarios);
            Assert.Equal(2, horarios.Count());
            Assert.Contains(horarios, h => h.HorarioDisponivelId == horario1.HorarioDisponivelId);
            Assert.Contains(horarios, h => h.HorarioDisponivelId == horario2.HorarioDisponivelId);
        }

        [Fact]
        public async Task Deve_Atualizar_Horario_Disponivel_Com_Sucesso()
        {
            // Arrange
            var horarioDisponivel = new HorarioDisponivel(
                Guid.NewGuid(),
                Guid.NewGuid(),
                1, // Segunda-feira
                TimeSpan.FromHours(9) // 09:00
            );

            await _repository.InserirAsync(horarioDisponivel);

            // Act
            horarioDisponivel.AtualizarHorario(2, TimeSpan.FromHours(14)); // Terça-feira, 14:00
            var resultadoAtualizar = await _repository.AtualizarAsync(horarioDisponivel);
            var horarioAtualizado = await _repository.ObterPorIdAsync(horarioDisponivel.HorarioDisponivelId);

            // Assert
            Assert.True(resultadoAtualizar);
            Assert.NotNull(horarioAtualizado);
            Assert.Equal(2, horarioAtualizado.DiaSemana);
            Assert.Equal(TimeSpan.FromHours(14), horarioAtualizado.Horas);
        }

        [Fact]
        public async Task Deve_Deletar_Horario_Disponivel_Com_Sucesso()
        {
            // Arrange
            var horarioDisponivel = new HorarioDisponivel(
                Guid.NewGuid(),
                Guid.NewGuid(),
                1, // Segunda-feira
                TimeSpan.FromHours(9) // 09:00
            );

            await _repository.InserirAsync(horarioDisponivel);

            // Act
            var resultadoDeletar = await _repository.DeletarAsync(horarioDisponivel.HorarioDisponivelId);

            // Assert
            Assert.True(resultadoDeletar);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _repository.ObterPorIdAsync(horarioDisponivel.HorarioDisponivelId));
        }

        [Fact]
        public async Task Deve_Verificar_Existencia_De_Horario_Com_Sucesso()
        {
            // Arrange
            var medicoId = Guid.NewGuid();
            var diaSemana = 1;
            var horas = TimeSpan.FromHours(9);
            var horarioDisponivel = new HorarioDisponivel(
                Guid.NewGuid(),
                medicoId,
                diaSemana,
                horas
            );

            await _repository.InserirAsync(horarioDisponivel);

            // Act
            var existe = await _repository.ExisteHorarioAsync(medicoId, diaSemana, horas);
            var naoExiste = await _repository.ExisteHorarioAsync(medicoId, diaSemana, TimeSpan.FromHours(10));

            // Assert
            Assert.True(existe);
            Assert.False(naoExiste);
        }
    }
} 