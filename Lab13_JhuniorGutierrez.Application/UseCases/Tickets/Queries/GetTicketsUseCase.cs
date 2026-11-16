using System;
using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Enums;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Tickets.Queries;

public class GetTicketsUseCase(IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyList<TicketDto>> ExecuteAsync(CurrentUser currentUser, TicketStatus? status = null)
    {
        var tickets = currentUser.IsStaff
            ? await unitOfWork.Tickets.GetAllWithUserAsync(status)
            : await unitOfWork.Tickets.GetByUserAsync(currentUser.UserId, status);

        return tickets.Select(TicketDto.FromEntity).ToList();
    }
}