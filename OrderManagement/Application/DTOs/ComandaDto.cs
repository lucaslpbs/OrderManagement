using OrderManagementAPI.Domain.Enums;
namespace OrderManagementAPI.Application.DTOs
{
    public class ComandaDto
    {
        public record AbrirComandaDto(int Numero, int Mesa, string NomeCliente, string? Email, string? Telefone);
        public record AdicionarItemDto(Guid ProdutoId, int Quantidade);
    }
}
