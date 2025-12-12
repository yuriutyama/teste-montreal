namespace TesteMonterealAPI.ViewModels
{
    public class LivroViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int AutorId { get; set; }
        public string AutorNome { get; set; } = string.Empty;
        public int GeneroId { get; set; }
        public string GeneroDescricao { get; set; } = string.Empty;
    }
}

