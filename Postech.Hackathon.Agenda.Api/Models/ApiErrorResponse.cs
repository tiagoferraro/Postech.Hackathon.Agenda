namespace Postech.Hackathon.Agenda.Api.Models;

public record ApiErrorResponse(int StatusCode, string Message, string? Details);
