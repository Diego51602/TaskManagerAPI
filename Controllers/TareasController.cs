using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/tareas")]
    [Authorize]
    public class TareasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TareasController(AppDbContext context)
        {
            _context = context;
        }

        private int GetUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetTareas()
        {
            var tareas = await _context.Tareas
                .Where(t => t.UsuarioId == GetUsuarioId())
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();

            return Ok(tareas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTarea(int id)
        {
            var tarea = await _context.Tareas
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == GetUsuarioId());

            if (tarea == null) return NotFound(new { message = "Tarea no encontrada" });

            return Ok(tarea);
        }

        [HttpPost]
        public async Task<IActionResult> CrearTarea([FromBody] TareaDto dto)
        {
            var tarea = new Tarea
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                UsuarioId = GetUsuarioId()
            };

            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id }, tarea);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTarea(int id, [FromBody] TareaDto dto)
        {
            var tarea = await _context.Tareas
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == GetUsuarioId());

            if (tarea == null) return NotFound(new { message = "Tarea no encontrada" });

            tarea.Titulo = dto.Titulo;
            tarea.Descripcion = dto.Descripcion;
            tarea.Completada = dto.Completada;
            tarea.FechaLimite = dto.FechaLimite;
            tarea.Categoria = dto.Categoria;

            await _context.SaveChangesAsync();

            return Ok(tarea);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTarea(int id)
        {
            var tarea = await _context.Tareas
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == GetUsuarioId());

            if (tarea == null) return NotFound(new { message = "Tarea no encontrada" });

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Tarea eliminada correctamente" });
        }
    }

    public record TareaDto(string Titulo, string Descripcion, bool Completada, DateTime? FechaLimite, string Categoria);
}