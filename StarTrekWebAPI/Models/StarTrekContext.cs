using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StarTrekWebAPI.Models;

public partial class StarTrekContext : DbContext
{
    public StarTrekContext()
    {
    }

    public StarTrekContext(DbContextOptions<StarTrekContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Spacecraft> Spacecrafts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(AppSettings.ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Spacecraft>(entity =>
        {
            entity.HasKey(e => e.Uid);

            entity.Property(e => e.Uid).HasColumnName("UID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
