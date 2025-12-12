using Entidade.Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repositorios.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Autor> Autores { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Livro> Livros { get; set; }
    }

}
