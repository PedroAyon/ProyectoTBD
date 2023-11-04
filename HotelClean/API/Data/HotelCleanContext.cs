using System;
using System.Collections.Generic;
using API.Domain.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace API.Data;

public partial class HotelCleanContext : DbContext
{
    private const string dbConnectionStringName = "HotelCleanDBConnection";
    private readonly IConfiguration _config;


    public HotelCleanContext(IConfiguration config)
    {
        _config = config;
    }

    public HotelCleanContext(IConfiguration config, DbContextOptions<HotelCleanContext> options)
        : base(options)
    {
        _config = config;
    }

    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<EmployeePerformance> Performances { get; set; }
    public virtual DbSet<Location> Locations { get; set; }
    public virtual DbSet<Service> Services { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connectionString = _config[$"ConnectionStrings:{dbConnectionStringName}"];
        if (connectionString != null)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.Parse("8.0.32-mysql"));
        }
        else
        {
            throw new Exception("Could not retrieve the database connection string");
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("employee");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(20);
            entity.Property(e => e.PhoneNumber).HasMaxLength(10);
            entity.Property(e => e.Position).HasColumnType("enum('Administración','Intendencia')");
            entity.Property(e => e.Status).HasColumnType("enum('Disponible','Ocupado','Inactivo')");
            entity.Property(e => e.Username).HasMaxLength(20);
        });

        modelBuilder.Entity<EmployeePerformance>(entity =>
        {
            entity.ToTable("EmployeePerformance");

            entity.HasKey(e => e.ID);

            entity.Property(e => e.Name)
                .HasColumnName("Name")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Status)
                .HasColumnName("Status")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(10);

            entity.Property(e => e.ServiceCount)
                .HasColumnName("ServiceCount");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("location");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Number).HasMaxLength(2);
            entity.Property(e => e.Type).HasColumnType("enum('Room','Area')");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("service");

            entity.HasIndex(e => e.LocationId, "FK_Service_Location");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EndingTime).HasColumnType("time");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.StartTime).HasColumnType("time");
            entity.Property(e => e.Status).HasColumnType("enum('Pendiente','En Curso','Terminado')");
            entity.Property(e => e.Type)
                .HasDefaultValueSql("'General'")
                .HasColumnType("enum('Limpieza','Sanitizacion','General')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
