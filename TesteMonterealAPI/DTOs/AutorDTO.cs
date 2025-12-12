using System.ComponentModel.DataAnnotations;

namespace TesteMonterealAPI.DTOs
{
    public class CreateAutorDTO
    {
        [Required(ErrorMessage = "O nome do autor é obrigatório")]
        [StringLength(200, ErrorMessage = "O nome do autor não pode exceder 200 caracteres")]
        public string Nome { get; set; } = string.Empty;
    }

    public class UpdateAutorDTO
    {
        [Required(ErrorMessage = "O nome do autor é obrigatório")]
        [StringLength(200, ErrorMessage = "O nome do autor não pode exceder 200 caracteres")]
        public string Nome { get; set; } = string.Empty;
    }
}

