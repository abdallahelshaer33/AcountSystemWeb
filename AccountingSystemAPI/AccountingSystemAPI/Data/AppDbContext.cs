using System;
using System.Collections.Generic;
using AccountingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystemAPI.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

    public virtual DbSet<JournalDetail> JournalDetails { get; set; }

    public virtual DbSet<JournalEntry> JournalEntries { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public DbSet<Account> accounts { get; set; }
    public DbSet<Transaction> transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=AccountingSystem;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07F4538E3C");

            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Expenses__3214EC07193B2717");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ExpenseDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Invoices__3214EC075F8D79AC");

            entity.Property(e => e.InvoiceDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoices__Custom__2B3F6F97");
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InvoiceI__3214EC073574CFC6");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceIt__Invoi__2E1BDC42");
        });

        modelBuilder.Entity<JournalDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JournalD__3214EC0796FB5CE6");

            entity.Property(e => e.AccountName).HasMaxLength(100);
            entity.Property(e => e.Credit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Debit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.JournalEntry).WithMany(p => p.JournalDetails)
                .HasForeignKey(d => d.JournalEntryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JournalDe__Journ__38996AB5");
        });

        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JournalE__3214EC070D196608");
            
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });
        modelBuilder.Entity<JournalEntry>()
        .HasMany(e => e.JournalDetails)
        .WithOne(e => e.JournalEntry)
        .HasForeignKey(d => d.JournalEntryId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC07DF453522");

            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07DF4DF481");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(100);
        });
        modelBuilder.Entity<Transaction>()
        .HasOne(t => t.DebitAccount)
        .WithMany()
        .HasForeignKey(t => t.DebitAccountId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Transaction>()
        .HasOne(t => t.CreditAccount)
        .WithMany()
        .HasForeignKey(t => t.CreditAccountId)
        .OnDelete(DeleteBehavior.Restrict);

        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Entity<Account>()
            .Property(t => t.OpeningBalance)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);

    }
   

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
