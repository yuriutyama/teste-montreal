using System;
using System.Collections.Generic;

namespace Entidade.Dominio
{
    public class Genero
    {
        public Genero()
        {
            Livros = new List<Livro>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public virtual ICollection<Livro> Livros { get; set; }
    }
}
