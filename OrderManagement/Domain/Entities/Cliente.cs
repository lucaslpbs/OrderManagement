namespace OrderManagementAPI.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; set; }
        public int NumeroComanda { get; set; } // Comanda vinculada
        public int Mesa { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefone { get; set; }

        public decimal ValorGasto { get; set; } = 0;
        public List<string> PedidosFeitos { get; set; } = new();
    }

}
