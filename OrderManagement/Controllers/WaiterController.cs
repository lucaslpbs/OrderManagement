using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementAPI.Services;
using static OrderManagementAPI.Application.DTOs.ComandaDto;

namespace OrderManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Garcom")]
    public class WaiterController : ControllerBase
    {
        private readonly OrderService _comandaService;

        public WaiterController(OrderService comandaService)
        {
            _comandaService = comandaService;
        }

        [HttpPost("abrir-comanda")]
        public async Task<IActionResult> AbrirComanda([FromBody] AbrirComandaDto dto)
        {
            var garcomId = User.FindFirst("sub")?.Value;
            if (garcomId == null) return Unauthorized();

            var comanda = await _comandaService.AbrirComandaAsync(dto.NumeroMesa, dto.NomeCliente, dto.Email, dto.Telefone);
            comanda.GarcomId = garcomId;
            return Ok(comanda);
        }

        [HttpGet("comandas")]
        public async Task<IActionResult> GetComandas()
        {
            var comandas = await _comandaService.GetAllAsync();
            return Ok(comandas);
        }

        [HttpGet("comanda/{id}")]
        public async Task<IActionResult> GetComanda(int id)
        {
            var comanda = await _comandaService.GetByIdAsync(id);
            if (comanda == null) return NotFound();
            return Ok(comanda);
        }

        [HttpPost("comanda/{id}/adicionar-item")]
        public async Task<IActionResult> AdicionarItem(int id, [FromBody] AdicionarItemDto dto)
        {
            var comanda = await _comandaService.AdicionarItemAsync(id, dto.ProdutoId, dto.Quantidade);
            return Ok(comanda);
        }

        [HttpPost("comanda/{id}/fechar")]
        public async Task<IActionResult> FecharComanda(int id, [FromBody] FecharComandaDto dto)
        {
            var comanda = await _comandaService.FecharComandaAsync(id, dto.Observacao);
            return Ok(comanda);
        }

        [HttpGet("atividades-recentes")]
        public async Task<IActionResult> AtividadesRecentes()
        {
            var garcomId = User.FindFirst("sub")?.Value;
            if (garcomId == null) return Unauthorized();

            var atividades = await _comandaService.GetAtividadesRecentesAsync(garcomId);
            return Ok(atividades);
        }
    }
}
