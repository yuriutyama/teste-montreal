using Microsoft.EntityFrameworkCore;
using Repositorios.Data;
using TesteMonterealAPI.DTOs;
using TesteMonterealAPI.ViewModels;
using Entidade.Dominio;

namespace TesteMonterealAPI.Services
{
    public class AutorService : IAutorService
    {
        private readonly ApplicationDbContext _context;

        public AutorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<IEnumerable<AutorViewModel>>> GetAllAsync()
        {
            try
            {
                var autores = await _context.Autores
                    .Include(a => a.Livros)
                    .Select(a => new AutorViewModel
                    {
                        Id = a.Id,
                        Nome = a.Nome,
                        TotalLivros = a.Livros.Count
                    })
                    .ToListAsync();

                return new ApiResponse<IEnumerable<AutorViewModel>>
                {
                    Success = true,
                    Message = "Autores recuperados com sucesso",
                    Data = autores
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<AutorViewModel>>
                {
                    Success = false,
                    Message = "Erro ao recuperar autores",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<AutorViewModel>> GetByIdAsync(int id)
        {
            try
            {
                var autor = await _context.Autores
                    .Include(a => a.Livros)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (autor == null)
                {
                    return new ApiResponse<AutorViewModel>
                    {
                        Success = false,
                        Message = "Autor não encontrado",
                        Errors = new List<string> { $"Autor com ID {id} não foi encontrado" }
                    };
                }

                var viewModel = new AutorViewModel
                {
                    Id = autor.Id,
                    Nome = autor.Nome,
                    TotalLivros = autor.Livros.Count
                };

                return new ApiResponse<AutorViewModel>
                {
                    Success = true,
                    Message = "Autor recuperado com sucesso",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AutorViewModel>
                {
                    Success = false,
                    Message = "Erro ao recuperar autor",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<AutorViewModel>> CreateAsync(CreateAutorDTO dto)
        {
            try
            {
                var autor = new Autor
                {
                    Nome = dto.Nome
                };

                _context.Autores.Add(autor);
                await _context.SaveChangesAsync();

                var viewModel = new AutorViewModel
                {
                    Id = autor.Id,
                    Nome = autor.Nome,
                    TotalLivros = 0
                };

                return new ApiResponse<AutorViewModel>
                {
                    Success = true,
                    Message = "Autor criado com sucesso",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AutorViewModel>
                {
                    Success = false,
                    Message = "Erro ao criar autor",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<AutorViewModel>> UpdateAsync(int id, UpdateAutorDTO dto)
        {
            try
            {
                var autor = await _context.Autores.FindAsync(id);

                if (autor == null)
                {
                    return new ApiResponse<AutorViewModel>
                    {
                        Success = false,
                        Message = "Autor não encontrado",
                        Errors = new List<string> { $"Autor com ID {id} não foi encontrado" }
                    };
                }

                autor.Nome = dto.Nome;
                await _context.SaveChangesAsync();

                var viewModel = new AutorViewModel
                {
                    Id = autor.Id,
                    Nome = autor.Nome,
                    TotalLivros = await _context.Livros.CountAsync(l => l.AutorId == autor.Id)
                };

                return new ApiResponse<AutorViewModel>
                {
                    Success = true,
                    Message = "Autor atualizado com sucesso",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AutorViewModel>
                {
                    Success = false,
                    Message = "Erro ao atualizar autor",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var autor = await _context.Autores
                    .Include(a => a.Livros)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (autor == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Autor não encontrado",
                        Errors = new List<string> { $"Autor com ID {id} não foi encontrado" }
                    };
                }

                if (autor.Livros.Any())
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Não é possível excluir autor com livros associados",
                        Errors = new List<string> { "O autor possui livros associados. Remova os livros antes de excluir o autor." }
                    };
                }

                _context.Autores.Remove(autor);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Autor excluído com sucesso",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Erro ao excluir autor",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}

