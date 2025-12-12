using Microsoft.EntityFrameworkCore;
using Repositorios.Data;
using TesteMonterealAPI.DTOs;
using TesteMonterealAPI.ViewModels;
using Entidade.Dominio;

namespace TesteMonterealAPI.Services
{
    public class LivroService : ILivroService
    {
        private readonly ApplicationDbContext _context;

        public LivroService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<IEnumerable<LivroViewModel>>> GetAllAsync()
        {
            try
            {
                var livros = await _context.Livros
                    .Include(l => l.Autor)
                    .Include(l => l.Genero)
                    .Select(l => new LivroViewModel
                    {
                        Id = l.Id,
                        Titulo = l.Titulo,
                        AutorId = l.AutorId,
                        AutorNome = l.Autor.Nome,
                        GeneroId = l.GeneroId,
                        GeneroDescricao = l.Genero.Descricao
                    })
                    .ToListAsync();

                return new ApiResponse<IEnumerable<LivroViewModel>>
                {
                    Success = true,
                    Message = "Livros recuperados com sucesso",
                    Data = livros
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<LivroViewModel>>
                {
                    Success = false,
                    Message = "Erro ao recuperar livros",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<LivroViewModel>> GetByIdAsync(int id)
        {
            try
            {
                var livro = await _context.Livros
                    .Include(l => l.Autor)
                    .Include(l => l.Genero)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (livro == null)
                {
                    return new ApiResponse<LivroViewModel>
                    {
                        Success = false,
                        Message = "Livro não encontrado",
                        Errors = new List<string> { $"Livro com ID {id} não foi encontrado" }
                    };
                }

                var viewModel = new LivroViewModel
                {
                    Id = livro.Id,
                    Titulo = livro.Titulo,
                    AutorId = livro.AutorId,
                    AutorNome = livro.Autor.Nome,
                    GeneroId = livro.GeneroId,
                    GeneroDescricao = livro.Genero.Descricao
                };

                return new ApiResponse<LivroViewModel>
                {
                    Success = true,
                    Message = "Livro recuperado com sucesso",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<LivroViewModel>
                {
                    Success = false,
                    Message = "Erro ao recuperar livro",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<LivroViewModel>> CreateAsync(CreateLivroDTO dto)
        {
            try
            {
                // Validar se autor existe
                var autor = await _context.Autores.FindAsync(dto.AutorId);
                if (autor == null)
                {
                    return new ApiResponse<LivroViewModel>
                    {
                        Success = false,
                        Message = "Autor não encontrado",
                        Errors = new List<string> { $"Autor com ID {dto.AutorId} não foi encontrado" }
                    };
                }

                // Validar se gênero existe
                var genero = await _context.Generos.FindAsync(dto.GeneroId);
                if (genero == null)
                {
                    return new ApiResponse<LivroViewModel>
                    {
                        Success = false,
                        Message = "Gênero não encontrado",
                        Errors = new List<string> { $"Gênero com ID {dto.GeneroId} não foi encontrado" }
                    };
                }

                var livro = new Livro
                {
                    Titulo = dto.Titulo,
                    AutorId = dto.AutorId,
                    GeneroId = dto.GeneroId
                };

                _context.Livros.Add(livro);
                await _context.SaveChangesAsync();

                // Recarregar com relacionamentos
                await _context.Entry(livro)
                    .Reference(l => l.Autor)
                    .LoadAsync();
                await _context.Entry(livro)
                    .Reference(l => l.Genero)
                    .LoadAsync();

                var viewModel = new LivroViewModel
                {
                    Id = livro.Id,
                    Titulo = livro.Titulo,
                    AutorId = livro.AutorId,
                    AutorNome = livro.Autor.Nome,
                    GeneroId = livro.GeneroId,
                    GeneroDescricao = livro.Genero.Descricao
                };

                return new ApiResponse<LivroViewModel>
                {
                    Success = true,
                    Message = "Livro criado com sucesso",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<LivroViewModel>
                {
                    Success = false,
                    Message = "Erro ao criar livro",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<LivroViewModel>> UpdateAsync(int id, UpdateLivroDTO dto)
        {
            try
            {
                var livro = await _context.Livros.FindAsync(id);

                if (livro == null)
                {
                    return new ApiResponse<LivroViewModel>
                    {
                        Success = false,
                        Message = "Livro não encontrado",
                        Errors = new List<string> { $"Livro com ID {id} não foi encontrado" }
                    };
                }

                // Validar se autor existe
                var autor = await _context.Autores.FindAsync(dto.AutorId);
                if (autor == null)
                {
                    return new ApiResponse<LivroViewModel>
                    {
                        Success = false,
                        Message = "Autor não encontrado",
                        Errors = new List<string> { $"Autor com ID {dto.AutorId} não foi encontrado" }
                    };
                }

                // Validar se gênero existe
                var genero = await _context.Generos.FindAsync(dto.GeneroId);
                if (genero == null)
                {
                    return new ApiResponse<LivroViewModel>
                    {
                        Success = false,
                        Message = "Gênero não encontrado",
                        Errors = new List<string> { $"Gênero com ID {dto.GeneroId} não foi encontrado" }
                    };
                }

                livro.Titulo = dto.Titulo;
                livro.AutorId = dto.AutorId;
                livro.GeneroId = dto.GeneroId;

                await _context.SaveChangesAsync();

                // Recarregar com relacionamentos
                await _context.Entry(livro)
                    .Reference(l => l.Autor)
                    .LoadAsync();
                await _context.Entry(livro)
                    .Reference(l => l.Genero)
                    .LoadAsync();

                var viewModel = new LivroViewModel
                {
                    Id = livro.Id,
                    Titulo = livro.Titulo,
                    AutorId = livro.AutorId,
                    AutorNome = livro.Autor.Nome,
                    GeneroId = livro.GeneroId,
                    GeneroDescricao = livro.Genero.Descricao
                };

                return new ApiResponse<LivroViewModel>
                {
                    Success = true,
                    Message = "Livro atualizado com sucesso",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<LivroViewModel>
                {
                    Success = false,
                    Message = "Erro ao atualizar livro",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var livro = await _context.Livros.FindAsync(id);

                if (livro == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Livro não encontrado",
                        Errors = new List<string> { $"Livro com ID {id} não foi encontrado" }
                    };
                }

                _context.Livros.Remove(livro);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Livro excluído com sucesso",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Erro ao excluir livro",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}

