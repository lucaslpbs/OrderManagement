//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNetCore.Identity;

//var hasher = new PasswordHasher<IdentityUser>();
//var user = new IdentityUser();
//string hash = hasher.HashPassword(user, "Admin@123"); // senha desejada
//Console.WriteLine(hash);


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Domain.Entities;
using OrderManagementAPI.Infrastructure.Data;

class Program
{
    static async Task Main()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(
                "server=localhost;port=3306;database=ordermanagementdb;user=root;password=MFIayej24@",
                new MySqlServerVersion(new Version(8, 0, 32))
            )
            .Options;

        using var db = new AppDbContext(options);

        var comandasExistentes = await db.Comanda.CountAsync();
        if (comandasExistentes > 0)
        {
            Console.WriteLine("Comandas já existem no banco. Nada foi feito.");
            return;
        }

        for (int i = 1; i <= 50; i++)
        {
            var comanda = new Comanda
            {
                Id = Guid.NewGuid(),
                Numero = i,
                Mesa = 0,
                NomeCliente = string.Empty,
                Email = string.Empty,
                Telefone = string.Empty,
                Status = true,
                DataAbertura = DateTime.MinValue
            };

            db.Comanda.Add(comanda);
            await db.SaveChangesAsync(); // salva uma por uma
            Console.WriteLine($"Comanda {i} criada com sucesso!");
        }

        Console.WriteLine("Todas as 50 comandas foram criadas!");
    }
}
