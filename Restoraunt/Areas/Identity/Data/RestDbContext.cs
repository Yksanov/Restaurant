using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restoraunt.Areas.Identity.Data;
using Restoraunt.Models;

namespace Restoraunt.Data
{
    public class RestDbContext : IdentityDbContext<RestorauntUser>
    {
        public RestDbContext(DbContextOptions<RestDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<ApplicationType> ApplicationType { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}
