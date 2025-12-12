using System;
using System.Collections.Generic;

namespace Entidade.Dominio
{
    public class Autor
    {
        public Autor()
        {
            Livros = new List<Livro>();
        }
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public virtual ICollection<Livro> Livros { get; set; }
    }
}
