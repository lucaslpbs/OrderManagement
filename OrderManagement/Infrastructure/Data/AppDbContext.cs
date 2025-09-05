using OrderManagementAPI.Domain.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace OrderManagementAPI.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Garcom> Garcom => Set<Garcom>();
        public DbSet<Produto> Produto => Set<Produto>();
        public DbSet<Comanda> Comanda => Set<Comanda>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
    }
}
