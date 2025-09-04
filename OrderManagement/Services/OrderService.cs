using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Domain.Entities;
using OrderManagementAPI.Infrastructure.Data;

namespace OrderManagementAPI.Services;

public class OrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Comanda> AbrirComandaAsync(int numeroMesa, string nomeCliente, string? email, string? telefone)
    {
        var comandaExistente = await _context.Comanda
            .FirstOrDefaultAsync(c => c.Mesa == numeroMesa && c.Status == true);

        if (comandaExistente != null)
            throw new InvalidOperationException("Já existe uma comanda aberta para esta mesa.");

        var comanda = new Comanda
        {
            Mesa = numeroMesa,
            NomeCliente = nomeCliente,
            Email = email,
            Telefone = telefone,
            DataAbertura = DateTime.UtcNow,
            Status = true
        };

        _context.Comanda.Add(comanda);
        await _context.SaveChangesAsync();

        return comanda;
    }

    public async Task<Comanda?> GetByIdAsync(int id)
    {
        return await _context.Comanda
            .Include(c => c.Itens).ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(c => c.Numero == id);
    }

    public async Task<IEnumerable<Comanda>> GetAllAsync()
    {
        return await _context.Comanda
            .Include(c => c.Itens)
            .OrderByDescending(c => c.DataAbertura)
            .ToListAsync();
    }

    public async Task<Comanda> AdicionarItemAsync(int comandaId, Guid produtoId, int quantidade)
    {
        var comanda = await _context.Comanda
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.Numero == comandaId && c.Status == true);

        if (comanda == null)
            throw new InvalidOperationException("Comanda não encontrada ou já fechada.");

        var produto = await _context.Produto.FirstOrDefaultAsync(p => p.Id == produtoId && p.Status == true);

        if (produto == null)
            throw new InvalidOperationException("Produto inválido ou inativo.");

        var item = new ComandaItem
        {
            ProdutoId = produto.Id,
            Quantidade = quantidade,
            ValorUnitario = produto.Preco
        };

        comanda.Itens.Add(item);
        await _context.SaveChangesAsync();

        return comanda;
    }

    public async Task<Comanda> FecharComandaAsync(int comandaId, string observacao)
    {
        var comanda = await _context.Comanda
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.Numero == comandaId && c.Status == true);

        if (comanda == null)
            throw new InvalidOperationException("Comanda não encontrada ou já fechada.");

        comanda.Status = false;
        comanda.DataFechamento = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return comanda;
    }

    public async Task<IEnumerable<Comanda>> GetAtividadesRecentesAsync(string garcomId)
    {
        return await _context.Comanda
            .Where(c => c.GarcomId == garcomId)
            .OrderByDescending(c => c.DataAbertura)
            .Take(10)
            .ToListAsync();
    }
}
