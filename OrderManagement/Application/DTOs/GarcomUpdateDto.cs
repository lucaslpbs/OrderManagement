namespace OrderManagementAPI.Application.DTOs
{
    public class GarcomUpdateDto
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public bool Status { get; set; }  // true = ativo, false = inativo
    }
}
