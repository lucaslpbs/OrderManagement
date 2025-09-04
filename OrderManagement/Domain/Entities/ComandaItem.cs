namespace OrderManagementAPI.Domain.Entities
{
    public class ComandaItem
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid ComandaId { get; set; }
        public Produto Produto { get; set; } = null!;
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Total => Quantidade * ValorUnitario;
    }
}
