using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Application.DTOs;
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
            var g = await _db.Garcons.FirstOrDefaultAsync(x => x.Id == id);
            if (g == null) return NotFound();
            g.NomeCompleto = dto.NomeCompleto;
            g.Telefone = dto.Telefone;
            g.Status = dto.Status;


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
            var g = await _db.Garcons.FirstOrDefaultAsync(x => x.Id == id);
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


        [HttpDelete("garcons/{id}")] // excluir acesso
        public async Task<IActionResult> ExcluirGarcom(Guid id)
        {
            var g = await _db.Garcons.FirstOrDefaultAsync(x => x.Id == id);
            if (g == null) return NotFound();


            var user = await _userMgr.FindByIdAsync(g.Id.ToString());
            if (user != null)
                await _userMgr.DeleteAsync(user);


            _db.Garcons.Remove(g);
            await _db.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("garcons")] // lista todos os garçons
        public async Task<ActionResult<IEnumerable<GarcomListItemDto>>> ListarGarcons()
        {
            var data = await _db.Garcons
            .OrderBy(g => g.NomeCompleto)
            .Select(g => new GarcomListItemDto(
            g.Id, g.NomeCompleto, g.NomeUsuario, g.Email, g.Telefone, g.Status, g.UltimoAcesso))
            .ToListAsync();
            return Ok(data);
        }
    }
}
