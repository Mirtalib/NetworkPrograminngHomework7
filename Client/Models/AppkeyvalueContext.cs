using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Client.Models;

public partial class AppkeyvalueContext : DbContext
{
    public AppkeyvalueContext()
    {
    }

    public AppkeyvalueContext(DbContextOptions<AppkeyvalueContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Keyvalue> Keyvalues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=APPKEYVALUE;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Keyvalue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KEYVALUE__3214EC07D7B5B10E");

            entity.ToTable("KEYVALUE");

            entity.Property(e => e.Value)
                .HasMaxLength(30)
                .HasColumnName("VALUE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
