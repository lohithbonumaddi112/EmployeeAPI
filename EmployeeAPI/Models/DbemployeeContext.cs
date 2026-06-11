using EmployeeAPI.Dto_s;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EmployeeAPI.Models;

public partial class DbemployeeContext : DbContext
{
    public DbemployeeContext()
    {
    }

    public DbemployeeContext(DbContextOptions<DbemployeeContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectModels;Database=EmployeeDB;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<Department> Department { get; set; }
    public virtual DbSet<EmployeeDepartmentDto> EmployeeDepartmentDtos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            modelBuilder.Entity<EmployeeDepartmentDto>().HasNoKey();
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC271E461C52");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Salary)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SALARY");
        });

        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<EmployeeDepartmentDto>().HasNoKey();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
