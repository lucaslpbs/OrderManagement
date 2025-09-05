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

    public async Task<Comanda> AbrirComandaAsync(int numero, int mesa, string nome, string? email, string? telefone)
    {
        var comanda = await _context.Comanda.FirstOrDefaultAsync(c => c.Numero == numero && c.Status == true);
        if (comanda == null) throw new Exception("Comanda já em uso ou inexistente.");

        // marca como em uso
        comanda.Status = false;
        comanda.Mesa = mesa;
        comanda.NomeCliente = nome;
        comanda.Email = email;
        comanda.Telefone = telefone;
        comanda.DataAbertura = DateTime.UtcNow;

        // cria registro de cliente associado
        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            NumeroComanda = numero,
            Mesa = mesa,
            NomeCliente = nome,
            Email = email,
            Telefone = telefone,
            ValorGasto = 0,
            PedidosFeitos = new List<string>()
        };
        _context.Clientes.Add(cliente);

        await _context.SaveChangesAsync();
        return comanda;
    }


    public async Task<Comanda?> GetByIdAsync(int id)
    {
        return await _context.Comanda
            .Include(c => c.Itens).ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(c => c.Numero == id);
    }
    public async Task<Comanda?> GetByNumeroAsync(int numero)
    {
        return await _context.Comanda
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.Numero == numero);
    }

    public async Task<IEnumerable<Comanda>> GetAllAsync()
    {
        return await _context.Comanda
            .Include(c => c.Itens)
            .OrderByDescending(c => c.DataAbertura)
            .ToListAsync();
    }

    public async Task<Comanda> AdicionarItemAsync(int numeroComanda, Guid produtoId, int quantidade)
    {
        var comanda = await _context.Comanda
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.Numero == numeroComanda && c.Status == false);

        if (comanda == null) throw new Exception("Comanda não está aberta.");

        // exemplo fictício de produto buscado
        var produto = await _context.Produto.FindAsync(produtoId);
        if (produto == null) throw new Exception("Produto não encontrado.");

        comanda.Itens.Add(new ComandaItem
        {
            Id = Guid.NewGuid(),
            ProdutoId = produtoId,
            Quantidade = quantidade,
            ValorUnitario = produto.Preco
        });

        // atualiza valor gasto do cliente
        var cliente = await _context.Clientes.FirstOrDefaultAsync(x => x.NumeroComanda == numeroComanda);
        if (cliente != null)
        {
            cliente.ValorGasto += produto.Preco * quantidade;
            cliente.PedidosFeitos.Add($"{produto.Nome} x{quantidade}");
        }

        await _context.SaveChangesAsync();
        return comanda;
    }

    public async Task<Comanda> FecharComandaAsync(int numeroComanda, string observacao)
    {
        var comanda = await _context.Comanda
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.Numero == numeroComanda && c.Status == false);

        if (comanda == null) throw new Exception("Comanda não está aberta.");

        // seta data de fechamento
        comanda.DataFechamento = DateTime.UtcNow;

        // resetar a comanda
        comanda.Mesa = 0;
        comanda.NomeCliente = string.Empty;
        comanda.Email = null;
        comanda.Telefone = null;
        comanda.Itens.Clear();
        comanda.Status = true;

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
