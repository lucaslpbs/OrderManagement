using OrderManagementAPI.Domain.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace OrderManagementAPI.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Garcom> Garcons => Set<Garcom>();
        public DbSet<Produto> Produtos => Set<Produto>();
        public DbSet<Comanda> Comandas => Set<Comanda>();
    }
}
