namespace OrderManagementAPI.Domain.Entities
{
    public class Comanda
    {
        public Guid Id { get; set; }
        public int Numero { get; set; }
        public int Mesa { get; set; }
        public string NomeCliente { get; set; } = null!;
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public DateTime DataAbertura { get; set; } = DateTime.UtcNow;
        public DateTime? DataFechamento { get; set; }
        public StatusComanda Status { get; set; } = StatusComanda.Aberta;

        public List<ComandaItem> Itens { get; set; } = new();
    }
}
