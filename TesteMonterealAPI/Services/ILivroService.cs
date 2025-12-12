using TesteMonterealAPI.DTOs;
using TesteMonterealAPI.ViewModels;

namespace TesteMonterealAPI.Services
{
    public interface ILivroService
    {
        Task<ApiResponse<IEnumerable<LivroViewModel>>> GetAllAsync();
        Task<ApiResponse<LivroViewModel>> GetByIdAsync(int id);
        Task<ApiResponse<LivroViewModel>> CreateAsync(CreateLivroDTO dto);
        Task<ApiResponse<LivroViewModel>> UpdateAsync(int id, UpdateLivroDTO dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}

