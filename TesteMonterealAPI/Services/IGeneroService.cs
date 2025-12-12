using TesteMonterealAPI.DTOs;
using TesteMonterealAPI.ViewModels;

namespace TesteMonterealAPI.Services
{
    public interface IGeneroService
    {
        Task<ApiResponse<IEnumerable<GeneroViewModel>>> GetAllAsync();
        Task<ApiResponse<GeneroViewModel>> GetByIdAsync(int id);
        Task<ApiResponse<GeneroViewModel>> CreateAsync(CreateGeneroDTO dto);
        Task<ApiResponse<GeneroViewModel>> UpdateAsync(int id, UpdateGeneroDTO dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}

