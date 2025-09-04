using OrderManagementAPI.Domain.Enums;
namespace OrderManagementAPI.Application.DTOs
{
    public class ComandaDto
    {
        public class AbrirComandaDto
        {
            public int NumeroMesa { get; set; }
            public string NomeCliente { get; set; } = string.Empty;
            public string? Email { get; set; }
            public string? Telefone { get; set; }
        }

        public class AdicionarItemDto
        {
            public Guid ProdutoId { get; set; }
            public int Quantidade { get; set; }
        }

        public class FecharComandaDto
        {
            public int ComandaId { get; set; }
            public string Observacao { get; set; } = string.Empty;
        }
    }
}
