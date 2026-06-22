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
    [Authorize] // Todos los endpoints requieren token JWT
    public class TareasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TareasController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene el ID del usuario autenticado desde el token JWT
        /// </summary>
        private int GetUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// <summary>
        /// Obtiene todas las tareas del usuario autenticado
        /// ordenadas por fecha de creación descendente
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetTareas()
        {
            var tareas = await _context.Tareas
                .Where(t => t.UsuarioId == GetUsuarioId())
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();

            return Ok(tareas);
        }

        /// <summary>
        /// Obtiene una tarea específica por ID
        /// Solo regresa la tarea si pertenece al usuario autenticado
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTarea(int id)
        {
            var tarea = await _context.Tareas
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == GetUsuarioId());

            if (tarea == null)
                return NotFound(new { message = "Tarea no encontrada" });

            return Ok(tarea);
        }

        /// <summary>
        /// Crea una nueva tarea para el usuario autenticado
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CrearTarea([FromBody] TareaDto dto)
        {
            var tarea = new Tarea
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                Completada = dto.Completada,
                FechaLimite = dto.FechaLimite,
                Categoria = dto.Categoria ?? "General",
                UsuarioId = GetUsuarioId()
            };

            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id }, tarea);
        }

        /// <summary>
        /// Actualiza una tarea existente
        /// Solo permite actualizar tareas del usuario autenticado
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTarea(int id, [FromBody] TareaDto dto)
        {
            var tarea = await _context.Tareas
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == GetUsuarioId());

            if (tarea == null)
                return NotFound(new { message = "Tarea no encontrada" });

            // Actualiza solo los campos permitidos
            tarea.Titulo = dto.Titulo;
            tarea.Descripcion = dto.Descripcion;
            tarea.Completada = dto.Completada;
            tarea.FechaLimite = dto.FechaLimite;
            tarea.Categoria = dto.Categoria ?? "General";

            await _context.SaveChangesAsync();

            return Ok(tarea);
        }

        /// <summary>
        /// Elimina una tarea por ID
        /// Solo permite eliminar tareas del usuario autenticado
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTarea(int id)
        {
            var tarea = await _context.Tareas
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == GetUsuarioId());

            if (tarea == null)
                return NotFound(new { message = "Tarea no encontrada" });

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Tarea eliminada correctamente" });
        }
    }
}