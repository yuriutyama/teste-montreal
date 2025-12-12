using Microsoft.EntityFrameworkCore;
using Repositorios.Data;
using TesteMonterealAPI.DTOs;
using TesteMonterealAPI.ViewModels;
using Entidade.Dominio;

namespace TesteMonterealAPI.Services
{
    public class GeneroService : IGeneroService
    {
        private readonly ApplicationDbContext _context;

        public GeneroService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<IEnumerable<GeneroViewModel>>> GetAllAsync()
        {
            try
            {
                var generos = await _context.Generos
                    .Include(g => g.Livros)
                    .Select(g => new GeneroViewModel
                    {
                        Id = g.Id,
                        Descricao = g.Descricao,
                        TotalLivros = g.Livros.Count
                    })
                    .ToListAsync();

                return new ApiResponse<IEnumerable<GeneroViewModel>>
                {
                    Success = true,
                    Message = "Gêneros recuperados com sucesso",
                    Data = generos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<GeneroViewModel>>
                {
                    Success = false,
                    Message = "Erro ao recuperar gêneros",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<GeneroViewModel>> GetByIdAsync(int id)
        {
            try
            {
                var genero = await _context.Generos
                    .Include(g => g.Livros)
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (genero == null)
                {
                    return new ApiResponse<GeneroViewModel>
                    {
                        Success = false,
                        Message = "Gênero não encontrado",
                        Errors = new List<string> { $"Gênero com ID {id} não foi encontrado" }
                    };
                }

                var viewModel = new GeneroViewModel
                {
                    Id = genero.Id,
                    Descricao = genero.Descricao,
                    TotalLivros = genero.Livros.Count
                };

                return new ApiResponse<GeneroViewModel>
                {
                    Success = true,
                    Message = "Gênero recuperado com sucesso",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<GeneroViewModel>
                {
                    Success = false,
                    Message = "Erro ao recuperar gênero",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<GeneroViewModel>> CreateAsync(CreateGeneroDTO dto)
        {
            try
            {
                var genero = new Genero
                {
                    Descricao = dto.Descricao
                };

                _context.Generos.Add(genero);
                await _context.SaveChangesAsync();

                var viewModel = new GeneroViewModel
                {
                    Id = genero.Id,
                    Descricao = genero.Descricao,
                    TotalLivros = 0
                };

                return new ApiResponse<GeneroViewModel>
                {
                    Success = true,
                    Message = "Gênero criado com sucesso",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<GeneroViewModel>
                {
                    Success = false,
                    Message = "Erro ao criar gênero",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<GeneroViewModel>> UpdateAsync(int id, UpdateGeneroDTO dto)
        {
            try
            {
                var genero = await _context.Generos.FindAsync(id);

                if (genero == null)
                {
                    return new ApiResponse<GeneroViewModel>
                    {
                        Success = false,
                        Message = "Gênero não encontrado",
                        Errors = new List<string> { $"Gênero com ID {id} não foi encontrado" }
                    };
                }

                genero.Descricao = dto.Descricao;
                await _context.SaveChangesAsync();

                var viewModel = new GeneroViewModel
                {
                    Id = genero.Id,
                    Descricao = genero.Descricao,
                    TotalLivros = await _context.Livros.CountAsync(l => l.GeneroId == genero.Id)
                };

                return new ApiResponse<GeneroViewModel>
                {
                    Success = true,
                    Message = "Gênero atualizado com sucesso",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<GeneroViewModel>
                {
                    Success = false,
                    Message = "Erro ao atualizar gênero",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var genero = await _context.Generos
                    .Include(g => g.Livros)
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (genero == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Gênero não encontrado",
                        Errors = new List<string> { $"Gênero com ID {id} não foi encontrado" }
                    };
                }

                if (genero.Livros.Any())
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Não é possível excluir gênero com livros associados",
                        Errors = new List<string> { "O gênero possui livros associados. Remova os livros antes de excluir o gênero." }
                    };
                }

                _context.Generos.Remove(genero);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Gênero excluído com sucesso",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Erro ao excluir gênero",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}

