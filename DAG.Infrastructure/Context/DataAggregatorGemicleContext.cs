using DAG.Domain.Customer;
using DAG.Domain.Events;
using DAG.Domain.Notifications;
using DAG.Domain.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAG.Infrastructure.Context;

public partial class DataAggregatorGemicleContext : DbContext
{
    public IConfiguration Configuration { get; set; }
    public DataAggregatorGemicleContext(DbContextOptions<DataAggregatorGemicleContext> options, IConfiguration configuration) : 
        base(options)
    {
        Configuration = configuration;
    }

    public DataAggregatorGemicleContext()
    {
        
    }

    public virtual DbSet<Customer101> Customer101s { get; set; }

    public virtual DbSet<Customer145> Customer145s { get; set; }

    public virtual DbSet<Customer2> Customer2s { get; set; }

    public virtual DbSet<EventTypes2> EventTypes2s { get; set; }

    public virtual DbSet<Events101> Events101s { get; set; }

    public virtual DbSet<Events145> Events145s { get; set; }

    public virtual DbSet<Events2> Events2s { get; set; }

    public virtual DbSet<NotificationsBroker> NotificationsBrokers { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DataAggregatorGemicleDatabase"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer101>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07EB40550F");

            entity.ToTable("Customer_101");

            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(128);
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(128);
            entity.Property(e => e.Salutation).HasMaxLength(10);
        });

        modelBuilder.Entity<Customer145>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Customer_145");

            entity.Property(e => e.Email).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Password).HasMaxLength(128);
            entity.Property(e => e.UserId).HasMaxLength(128);
        });

        modelBuilder.Entity<Customer2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07C9E0D49C");

            entity.ToTable("Customer_2");

            entity.Property(e => e.Email).HasMaxLength(128);
            entity.Property(e => e.JobPosition).HasMaxLength(128);
            entity.Property(e => e.PasswordHash).HasMaxLength(128);
        });

        modelBuilder.Entity<EventTypes2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventTyp__3214EC072D2615C7");

            entity.ToTable("EventTypes_2");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(64);
        });

        modelBuilder.Entity<Events101>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Events_1__3214EC07267F42BC");

            entity.ToTable("Events_101");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.EventDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Events145>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Events_1__3214EC070EED6F3D");

            entity.ToTable("Events_145");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CustomerId);
            entity.Property(e => e.EventDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Events2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Events_2__3214EC07A79330B6");

            entity.ToTable("Events_2");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.EventDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<NotificationsBroker>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("NotificationsBroker");

            entity.Property(e => e.Email).HasMaxLength(128);
            entity.Property(e => e.FinHash).HasMaxLength(128);
            entity.Property(e => e.FirstName).HasMaxLength(128);
            entity.Property(e => e.LastName).HasMaxLength(128);
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tenants__3214EC0708B11679");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.OrganisationName).HasMaxLength(128);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}