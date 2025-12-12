using System;

namespace Entidade.Dominio
{
    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int GeneroId { get; set; }
        public virtual Genero Genero { get; set; } = null!;
        public int AutorId { get; set; }
        public virtual Autor Autor { get; set; } = null!;
    }
}
