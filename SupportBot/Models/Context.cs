﻿using Microsoft.EntityFrameworkCore;

namespace SupportBot.Models;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    private readonly string conconnection;

    public ApplicationContext(string conconnection)
    {
        this.conconnection = conconnection;
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(conconnection);
    }
}
