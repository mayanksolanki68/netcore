using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdminPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Gender> Genders{ get; set; }
        public DbSet<Address> Address { get; set; }
    }
}
