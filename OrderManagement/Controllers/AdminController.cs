using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Application.DTOs;
using OrderManagementAPI.Domain.Entities;
using OrderManagementAPI.Infrastructure.Data;

namespace OrderManagementAPI.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController(AppDbContext db, UserManager<IdentityUser> userMgr) : ControllerBase
    {
        private readonly AppDbContext _db = db;
        private readonly UserManager<IdentityUser> _userMgr = userMgr;

        [HttpPut("garcons/{id}")] // editar
        public async Task<IActionResult> EditarGarcom(Guid id, [FromBody] GarcomUpdateDto dto)
        {
            var g = await _db.Garcom.FirstOrDefaultAsync(x => x.Id == id);
            if (g == null) return NotFound();
            g.NomeCompleto = dto.NomeCompleto;
            g.NomeUsuario = dto.NomeUsuario;
            g.Telefone = dto.Telefone;


            var user = await _userMgr.FindByIdAsync(g.Id.ToString());
            if (user != null)
            {
                user.PhoneNumber = dto.Telefone;
                // trava/destrava login conforme status
                if (!dto.Status)
                    user.LockoutEnd = DateTimeOffset.MaxValue;
                else
                    user.LockoutEnd = null;
                await _userMgr.UpdateAsync(user);
            }


            await _db.SaveChangesAsync();
            return NoContent();
        }


        [HttpPut("garcons/{id}/desativar")] // desativar acesso
        public async Task<IActionResult> DesativarGarcom(Guid id)
        {
            var g = await _db.Garcom.FirstOrDefaultAsync(x => x.Id == id);
            if (g == null) return NotFound();
            g.Status = false;


            var user = await _userMgr.FindByIdAsync(g.Id.ToString());
            if (user != null)
            {
                user.LockoutEnd = DateTimeOffset.MaxValue;
                await _userMgr.UpdateAsync(user);
            }


            await _db.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("garcons/{id}")]
        public async Task<IActionResult> ExcluirGarcom(Guid id)
        {
            var g = await _db.Garcom.FirstOrDefaultAsync(x => x.Id == id);
            if (g == null) return NotFound();

            var user = await _userMgr.FindByIdAsync(g.Id.ToString());
            if (user != null)
            {
                await _userMgr.DeleteAsync(user);
            }

            // recarrega para garantir que ainda existe
            g = await _db.Garcom.FirstOrDefaultAsync(x => x.Id == id);
            if (g != null)
            {
                _db.Garcom.Remove(g);
                await _db.SaveChangesAsync();
            }

            return NoContent();
        }


        [HttpGet("garcons")] // lista todos os garçons
        public async Task<ActionResult<IEnumerable<GarcomListItemDto>>> ListarGarcons()
        {
            var data = await _db.Garcom
            .OrderBy(g => g.NomeCompleto)
            .Select(g => new GarcomListItemDto(
            g.Id, g.NomeCompleto, g.NomeUsuario, g.Email, g.Telefone, g.Status, g.UltimoAcesso))
            .ToListAsync();
            return Ok(data);
        }

        [HttpPost("garcons")] // criar novo garçom
        public async Task<IActionResult> CriarGarcom([FromBody] GarcomDto dto)
        {
            // 1) Criar usuário no Identity
            var user = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(), // garante compatibilidade com AspNetUsers
                UserName = dto.NomeUsuario,
                Email = dto.Email,
                PhoneNumber = dto.Telefone,
                EmailConfirmed = true
            };

            var result = await _userMgr.CreateAsync(user, dto.SenhaHash);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Adicionar à role Garcom
            await _userMgr.AddToRoleAsync(user, "Garcom");

            // 2) Criar registro na tabela de Garçom usando o mesmo Id do IdentityUser
            var garcom = new Garcom
            {
                Id = Guid.Parse(user.Id), // sincroniza com o IdentityUser
                NomeCompleto = dto.NomeCompleto,
                NomeUsuario = dto.NomeUsuario,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Status = true,
                UltimoAcesso = DateTime.UtcNow
            };

            _db.Garcom.Add(garcom);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(ListarGarcons), new { id = garcom.Id }, new
            {
                garcom.Id,
                garcom.NomeCompleto,
                garcom.NomeUsuario,
                garcom.Email,
                garcom.Telefone,
                garcom.Status
            });
        }

    }
}
