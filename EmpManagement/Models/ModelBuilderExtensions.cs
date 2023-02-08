using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
               new Employee
               {
                   Id = 1,
                   Name = "Mark",
                   Department = Dept.IT,
                   Email = "mark@gmail.com"
               }
           );
            modelBuilder.Entity<Employee>().HasData(
               new Employee
               {
                   Id = 2,
                   Name = "John",
                   Department = Dept.Sales,
                   Email = "john@gmail.com"
               }
           );
        }
    }
}
