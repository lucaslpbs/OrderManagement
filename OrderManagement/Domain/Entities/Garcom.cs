namespace OrderManagementAPI.Domain.Entities
{
    public class Garcom
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; } = null!;
        public string NomeUsuario { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Telefone { get; set; } = null!;
        public bool Status { get; set; } = true;
        public DateTime? UltimoAcesso { get; set; }

    }
}
