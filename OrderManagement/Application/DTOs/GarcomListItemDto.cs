namespace OrderManagementAPI.Application.DTOs
{
    public class GarcomListItemDto(Guid id, string nomeCompleto, string nomeUsuario, string email, string telefone, bool status, DateTime? ultimoAcesso)
    {
        public Guid Id { get; set; } = id;
        public string NomeCompleto { get; set; } = nomeCompleto;
        public string Usuario { get; set; } = nomeUsuario;
        public string Email { get; set; } = email;
        public string Telefone { get; set; } = telefone;
        public bool Status { get; set; } = status;
        public DateTime? UltimoAcesso { get; set; } = ultimoAcesso;
    }
}
