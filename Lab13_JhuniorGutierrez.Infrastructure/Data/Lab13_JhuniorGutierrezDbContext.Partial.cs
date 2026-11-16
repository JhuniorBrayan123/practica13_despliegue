using Lab13_JhuniorGutierrez.Domain.Models;
using Lab13_JhuniorGutierrez.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;

namespace Lab13_JhuniorGutierrez.Infrastructure.Data;

public partial class Lab13_JhuniorGutierrezDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>()
            .Property(t => t.Status)
            .HasConversion(new TicketStatusConverter());
    }
}
