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
            //var garcomId = User.FindFirst("sub")?.Value;
            //if (garcomId == null) return Unauthorized();

            var comanda = await _comandaService.AbrirComandaAsync(
                dto.Numero,
                dto.Mesa,
                dto.NomeCliente,
                dto.Email,
                dto.Telefone
            );

            //comanda.GarcomId = garcomId;
            return Ok(comanda);
        }

        [HttpGet("comandas")]
        public async Task<IActionResult> GetComandas()
        {
            var comandas = await _comandaService.GetAllAsync();
            return Ok(comandas);
        }

        [HttpGet("comanda/{numero}")]
        public async Task<IActionResult> GetComanda(int numero)
        {
            var comanda = await _comandaService.GetByNumeroAsync(numero);
            if (comanda == null) return NotFound();
            return Ok(comanda);
        }

        [HttpPost("comanda/{numero}/adicionar-item")]
        public async Task<IActionResult> AdicionarItem(int numero, [FromBody] AdicionarItemDto dto)
        {
            var comanda = await _comandaService.AdicionarItemAsync(numero, dto.ProdutoId, dto.Quantidade);
            return Ok(comanda);
        }

        [HttpPost("comanda/{numero}/fechar")]
        public async Task<IActionResult> FecharComanda(int numero, [FromBody] FecharComandaDto dto)
        {
            var comanda = await _comandaService.FecharComandaAsync(numero, dto.Observacao);
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
