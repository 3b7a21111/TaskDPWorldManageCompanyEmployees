using EmployeeManagement.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DAL.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {   
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //عشان اعمل user for role Admin first time 
            //var hasher = new PasswordHasher<User>();

            //var adminUser = new User
            //{
            //    Id = 1,
            //    Username = "admin",
            //    Password = "",  
            //    Role = "Admin"
            //};

            //adminUser.Password = hasher.HashPassword(adminUser, "admin123");

            //modelBuilder.Entity<User>().HasData(adminUser);

            #region 
            //relation between tables (fluent api) 
            //[one to one]
            //modelBuilder.Entity<User>()
            //    .HasOne(u => u.Employee)
            //    .WithOne(e => e.User)
            //    .HasForeignKey<Employee>(e => e.UserId);
            //[one to many]
            //modelBuilder.Entity<Attendance>()
            //    .HasOne(a => a.Employee)
            //    .WithMany(e => e.Attendances)
            //    .HasForeignKey(a => a.EmployeeId);
            #endregion
        }

    }
}
