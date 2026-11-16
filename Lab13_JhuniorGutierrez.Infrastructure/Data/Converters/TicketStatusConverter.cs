using Lab13_JhuniorGutierrez.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lab13_JhuniorGutierrez.Infrastructure.Data.Converters;

public class TicketStatusConverter() : ValueConverter<TicketStatus, string>(
    status => status == TicketStatus.Abierto ? "abierto"
        : status == TicketStatus.EnProceso ? "en_proceso"
        : "cerrado",
    value => value == "abierto" ? TicketStatus.Abierto
        : value == "en_proceso" ? TicketStatus.EnProceso
        : TicketStatus.Cerrado);
