using OrderManagementAPI.Domain.Enums;
namespace OrderManagementAPI.Domain.Entities
{
    public class Produto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public decimal Preco { get; set; }
        public CategoriaProduto Categoria { get; set; }
        public string? ImagemBase64 { get; set; }
        public bool Status { get; set; } = true;
    }
}
