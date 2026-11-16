using System.ComponentModel.DataAnnotations;
using Lab13_JhuniorGutierrez.Domain.Enums;
using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.Dtos;

public record TicketDto(
    Guid TicketId,
    Guid UserId,
    string Username,
    string Title,
    string? Description,
    TicketStatus Status,
    DateTime? CreatedAt,
    DateTime? ClosedAt)
{
    public static TicketDto FromEntity(Ticket ticket) => new(
        ticket.TicketId,
        ticket.UserId,
        ticket.User.Username,
        ticket.Title,
        ticket.Description,
        ticket.Status,
        ticket.CreatedAt,
        ticket.ClosedAt);
}

public record CreateTicketRequest(
    [Required, StringLength(255, MinimumLength = 3)] string Title,
    string? Description);

public record UpdateTicketRequest(
    [Required, StringLength(255, MinimumLength = 3)] string Title,
    string? Description);

public record ChangeTicketStatusRequest([Required] TicketStatus? Status);
