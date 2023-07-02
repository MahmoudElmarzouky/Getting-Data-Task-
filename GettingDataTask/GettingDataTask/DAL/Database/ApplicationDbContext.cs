using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MicrotechTask.DAL.Entities;

namespace MicrotechTask.DAL.Database;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccNumber);

            entity.Property(e => e.AccNumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Acc_Number");
            entity.Property(e => e.AccParent)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("ACC_Parent");
            entity.Property(e => e.Balance).HasColumnType("decimal(20, 9)");

            entity.HasOne(d => d.AccParentNavigation).WithMany(p => p.InverseAccParentNavigation)
                .HasForeignKey(d => d.AccParent)
                .HasConstraintName("FK_Accounts_Accounts");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
