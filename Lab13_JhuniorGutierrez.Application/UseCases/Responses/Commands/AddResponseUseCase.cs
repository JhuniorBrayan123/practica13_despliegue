using System;
using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Application.UseCases.Tickets;
using Lab13_JhuniorGutierrez.Domain.Enums;
using Lab13_JhuniorGutierrez.Domain.Exceptions;
using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Responses.Commands;

public class AddResponseUseCase(IUnitOfWork unitOfWork)
{
    public async Task<ResponseDto> ExecuteAsync(Guid ticketId, CreateResponseRequest request, CurrentUser currentUser)
    {
        var ticket = await TicketAccess.GetAuthorizedAsync(unitOfWork, ticketId, currentUser);

        if (ticket.Status == TicketStatus.Cerrado)
            throw new DomainException("A closed ticket cannot receive responses.");

        if (currentUser.IsStaff && ticket.Status == TicketStatus.Abierto)
            ticket.Status = TicketStatus.EnProceso;

        var response = new Response
        {
            ResponseId = Guid.NewGuid(),
            TicketId = ticket.TicketId,
            ResponderId = currentUser.UserId,
            Message = request.Message.Trim()
        };

        await unitOfWork.Responses.AddAsync(response);
        unitOfWork.Tickets.Update(ticket);
        await unitOfWork.SaveChangesAsync();

        var responses = await unitOfWork.Responses.GetByTicketAsync(ticketId);
        return ResponseDto.FromEntity(responses.First(r => r.ResponseId == response.ResponseId));
    }
}