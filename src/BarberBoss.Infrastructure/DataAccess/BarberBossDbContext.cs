using BarberBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess;

public class BarberBossDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Billing> Billings { get; set; }
    public DbSet<User> Users { get; set; }
}