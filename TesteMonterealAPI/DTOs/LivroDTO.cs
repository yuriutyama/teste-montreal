using System.ComponentModel.DataAnnotations;

namespace TesteMonterealAPI.DTOs
{
    public class CreateLivroDTO
    {
        [Required(ErrorMessage = "O título do livro é obrigatório")]
        [StringLength(300, ErrorMessage = "O título do livro não pode exceder 300 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID do autor é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID do autor deve ser maior que zero")]
        public int AutorId { get; set; }

        [Required(ErrorMessage = "O ID do gênero é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID do gênero deve ser maior que zero")]
        public int GeneroId { get; set; }
    }

    public class UpdateLivroDTO
    {
        [Required(ErrorMessage = "O título do livro é obrigatório")]
        [StringLength(300, ErrorMessage = "O título do livro não pode exceder 300 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID do autor é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID do autor deve ser maior que zero")]
        public int AutorId { get; set; }

        [Required(ErrorMessage = "O ID do gênero é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID do gênero deve ser maior que zero")]
        public int GeneroId { get; set; }
    }
}

