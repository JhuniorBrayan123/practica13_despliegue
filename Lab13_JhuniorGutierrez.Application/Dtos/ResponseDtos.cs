using System.ComponentModel.DataAnnotations;
using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.Dtos;

public record ResponseDto(Guid ResponseId, Guid TicketId, Guid ResponderId, string ResponderUsername, string Message, DateTime? CreatedAt)
{
    public static ResponseDto FromEntity(Response response) => new(
        response.ResponseId,
        response.TicketId,
        response.ResponderId,
        response.Responder.Username,
        response.Message,
        response.CreatedAt);
}

public record CreateResponseRequest([Required, MinLength(1)] string Message);

public record UpdateResponseRequest([Required, MinLength(1)] string Message);
