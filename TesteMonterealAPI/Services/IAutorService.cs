using TesteMonterealAPI.DTOs;
using TesteMonterealAPI.ViewModels;

namespace TesteMonterealAPI.Services
{
    public interface IAutorService
    {
        Task<ApiResponse<IEnumerable<AutorViewModel>>> GetAllAsync();
        Task<ApiResponse<AutorViewModel>> GetByIdAsync(int id);
        Task<ApiResponse<AutorViewModel>> CreateAsync(CreateAutorDTO dto);
        Task<ApiResponse<AutorViewModel>> UpdateAsync(int id, UpdateAutorDTO dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}

