# Sistema de Agendamento Médico

Este é um sistema de agendamento médico desenvolvido como parte do Hackathon da Postech. O sistema permite o gerenciamento de agendamentos entre médicos e pacientes, com funcionalidades para criar, aprovar e recusar consultas.

## Funcionalidades

### Agendamentos
- Criar novo agendamento
- Obter agendamento por ID
- Listar agendamentos por médico
- Listar agendamentos por paciente
- Aprovar agendamento
- Recusar agendamento

### Horários Disponíveis
- Criar horário disponível
- Alterar horário disponível
- Listar horários disponíveis por médico

## Tecnologias Utilizadas

- .NET 9.0
- ASP.NET Core
- SQL Server
- Dapper
- JWT Authentication
- OpenTelemetry
- Prometheus

## Estrutura do Projeto

O projeto está organizado em camadas:

- **Api**: Camada de apresentação com os controllers
- **Application**: Camada de aplicação com serviços e DTOs
- **Domain**: Camada de domínio com entidades e enums
- **Infra**: Camada de infraestrutura com repositórios
- **Ioc**: Configuração de injeção de dependência
- **Test**: Testes unitários
- **TestIntegration**: Testes de integração

## Autenticação e Autorização

O sistema utiliza autenticação JWT com os seguintes perfis:
- Médico
- Paciente
- Administrador

## Métricas e Monitoramento

O sistema está instrumentado com:
- OpenTelemetry para rastreamento distribuído
- Prometheus para coleta de métricas
- Métricas HTTP e de runtime


## Endpoints

### Agendamentos
- `POST /api/agendamento` - Criar novo agendamento
- `GET /api/agendamento/{id}` - Obter agendamento por ID
- `POST /api/agendamento/obterpormedico` - Listar agendamentos por médico
- `POST /api/agendamento/obterporpaciente` - Listar agendamentos por paciente
- `POST /api/agendamento/recusar` - Recusar agendamento
- `POST /api/agendamento/aprovar` - Aprovar agendamento

### Horários Disponíveis
- `POST /api/horariodisponivel` - Criar horário disponível
- `PUT /api/horariodisponivel` - Alterar horário disponível
- `GET /api/horariodisponivel/obterhorariospormedico/{medicoId}` - Listar horários por médico 