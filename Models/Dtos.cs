using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    // DTO para registrar un usuario nuevo
    public record RegisterDto(
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
        string Nombre,

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        string Email,

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener mínimo 6 caracteres")]
        string Password
    );

    // DTO para hacer login
    public record LoginDto(
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        string Email,

        [Required(ErrorMessage = "La contraseña es requerida")]
        string Password
    );

    // DTO para crear o actualizar una tarea
    public record TareaDto(
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El título debe tener entre 1 y 100 caracteres")]
        string Titulo,

        [StringLength(300, ErrorMessage = "La descripción no puede exceder 300 caracteres")]
        string Descripcion,

        bool Completada,

        DateTime? FechaLimite,

        [StringLength(50)]
        string Categoria
    );
}