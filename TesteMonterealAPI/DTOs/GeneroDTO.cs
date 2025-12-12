using System.ComponentModel.DataAnnotations;

namespace TesteMonterealAPI.DTOs
{
    public class CreateGeneroDTO
    {
        [Required(ErrorMessage = "A descrição do gênero é obrigatória")]
        [StringLength(100, ErrorMessage = "A descrição do gênero não pode exceder 100 caracteres")]
        public string Descricao { get; set; } = string.Empty;
    }

    public class UpdateGeneroDTO
    {
        [Required(ErrorMessage = "A descrição do gênero é obrigatória")]
        [StringLength(100, ErrorMessage = "A descrição do gênero não pode exceder 100 caracteres")]
        public string Descricao { get; set; } = string.Empty;
    }
}

