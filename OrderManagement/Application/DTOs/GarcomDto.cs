namespace OrderManagementAPI.Application.DTOs
{
    public class GarcomDto
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; } = null!;
        public string NomeUsuario { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Telefone { get; set; } = null!;
        public string SenhaHash { get; set; } = null!;
        public bool Ativo { get; set; } = true;
        public DateTime? UltimoAcesso { get; set; }
    }
}
