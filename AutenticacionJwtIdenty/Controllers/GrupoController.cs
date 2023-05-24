using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutenticacionJwtIdenty.Context;
using AutenticacionJwtIdenty.Models;
using Microsoft.AspNetCore.Authorization;

namespace AutenticacionJwtIdenty.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GrupoController : ControllerBase
    {
        private readonly BdContext _context;

        public GrupoController(BdContext context)
        {
            _context = context;
        }

        // GET: api/Grupo
        [Authorize(Roles = "User")]
        [Authorize(Policy = "PermisoCrearUsuario")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grupo>>> GetGrupos()
        {
            if (_context.Grupos == null)
            {
                return NotFound();
            }
            return await _context.Grupos.ToListAsync();
        }

        // GET: api/Grupo/5
        //[Authorize(Roles = "User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Grupo>> GetGrupo(int id)
        {
            if (_context.Grupos == null)
            {
                return NotFound();
            }
            var grupo = await _context.Grupos.FindAsync(id);

            if (grupo == null)
            {
                return NotFound();
            }

            return grupo;
        }

        // PUT: api/Grupo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrupo(int id, Grupo grupo)
        {
            if (id != grupo.GrupoId)
            {
                return BadRequest();
            }

            _context.Entry(grupo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrupoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Grupo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Grupo>> PostGrupo(Grupo grupo)
        {
            if (_context.Grupos == null)
            {
                return Problem("Entity set 'BdContext.Grupos'  is null.");
            }
            _context.Grupos.Add(grupo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGrupo", new { id = grupo.GrupoId }, grupo);
        }

        // DELETE: api/Grupo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrupo(int id)
        {
            if (_context.Grupos == null)
            {
                return NotFound();
            }
            //var grupo = await _context.Grupos.Where(g => g.GrupoId == id).FirstOrDefaultAsync();
            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo == null)
            {
                return NotFound();
            }

            _context.Grupos.Remove(grupo);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool GrupoExists(int id)
        {
            return (_context.Grupos?.Any(e => e.GrupoId == id)).GetValueOrDefault();
        }
    }
}
