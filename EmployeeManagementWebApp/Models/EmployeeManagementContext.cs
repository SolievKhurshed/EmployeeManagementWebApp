using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.Models
{
    public class EmployeeManagementContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {                                                          
            options.UseSqlite($"Data Source={AppDomain.CurrentDomain.BaseDirectory}\\EmpManagement.db");
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<EmployeePosition> EmployeePositions { get; set; }
        public DbSet<MobilePhone> MobilePhones { get; set; }
        
        public DbSet<AppLog> AppLogs { get; set; }
        public DbSet<AppLogType> AppLogTypes { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(ConfigureDepartment);
            modelBuilder.Entity<Group>(ConfigureGroup);
            modelBuilder.Entity<Employee>(ConfigureEmployee);
            modelBuilder.Entity<Position>(ConfigurePosition);
            modelBuilder.Entity<EmployeePosition>(ConfigureEmployeePosition);
            modelBuilder.Entity<MobilePhone>(ConfigureMobilePhone);

            modelBuilder.Entity<AppLog>(ConfigureAppLog);
            modelBuilder.Entity<AppLogType>(ConfigureAppLogType);
        }

        private void ConfigureDepartment(EntityTypeBuilder<Department> builder)
        {
            builder.Property(c => c.Name)
               .IsRequired(true)
               .HasColumnType("nvarchar(255)");

            builder.Property(c => c.Email)
                .IsRequired(false)
                .HasColumnType("nvarchar(50)");

            builder.Property(c => c.Description)
                .IsRequired(false)
                .HasColumnType("nvarchar(1024)");
        }

        private void ConfigureGroup(EntityTypeBuilder<Group> builder)
        {
            builder.Property(c => c.Name)
               .IsRequired(true)
               .HasColumnType("nvarchar(255)");

            builder.Property(c => c.Email)
                .IsRequired(false)
                .HasColumnType("nvarchar(50)");

            builder.Property(c => c.Description)
                .IsRequired(false)
                .HasColumnType("nvarchar(1024)");
        }

        private void ConfigureEmployee(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(c => c.FirstName)
               .IsRequired(true)
               .HasColumnType("nvarchar(100)");

            builder.Property(c => c.LastName)
               .IsRequired(true)
               .HasColumnType("nvarchar(100)");

            builder.Property(c => c.MiddleName)
               .IsRequired(false)
               .HasColumnType("nvarchar(100)");

            builder.Property(c => c.Login)
               .IsRequired(true)
               .HasColumnType("nvarchar(50)");

            builder.Property(c => c.Email)
               .IsRequired(true)
               .HasColumnType("nvarchar(50)");

            builder.Property(c => c.ExtensionPhone)
               .IsRequired(false)
               .HasColumnType("int");

            builder.Property(c => c.BirthDate)
               .IsRequired(false)
               .HasColumnType("datetime");

            builder.Property(c => c.EmploymentDate)
               .IsRequired(false)
               .HasColumnType("datetime");

            builder.Property(c => c.IsActive)
              .IsRequired(true)
              .HasColumnType("boolean");
        }

        private void ConfigurePosition(EntityTypeBuilder<Position> builder)
        {
            builder.Property(c => c.Name)
               .IsRequired(true)
               .HasColumnType("nvarchar(255)");
        }

        private void ConfigureEmployeePosition(EntityTypeBuilder<EmployeePosition> builder)
        {
            builder
                .HasKey(c => new { c.EmployeeId, c.PositionId });

            builder
                .HasOne(ep => ep.Employee)
                .WithMany(e => e.EmployeePositions)
                .HasForeignKey(ep => ep.EmployeeId);

            builder
                .HasOne(ep => ep.Position)
                .WithMany(p => p.EmployeePositions)
                .HasForeignKey(ep => ep.PositionId);
        }

        private void ConfigureMobilePhone(EntityTypeBuilder<MobilePhone> builder)
        {
            builder.Property(c => c.Number)
               .IsRequired(true)
               .HasColumnType("nvarchar(12)");
        }
        
        private void ConfigureAppLog(EntityTypeBuilder<AppLog> builder)
        {
            builder.Property(c => c.Created)
               .IsRequired(true)
               .HasColumnType("datetime");
        }

        private void ConfigureAppLogType(EntityTypeBuilder<AppLogType> builder)
        {
            builder.Property(c => c.TypeName)
               .IsRequired(true)
               .HasColumnType("nvarchar(200)");
        }
    }
}