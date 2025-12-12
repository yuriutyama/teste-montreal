using Microsoft.AspNetCore.Mvc;
using TesteMonterealAPI.DTOs;
using TesteMonterealAPI.Services;
using TesteMonterealAPI.ViewModels;

namespace TesteMonterealAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class LivrosController : ControllerBase
    {
        private readonly ILivroService _livroService;

        public LivrosController(ILivroService livroService)
        {
            _livroService = livroService;
        }

        /// <summary>
        /// Obtém todos os livros
        /// </summary>
        /// <returns>Lista de livros</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LivroViewModel>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<LivroViewModel>>>> GetAll()
        {
            var response = await _livroService.GetAllAsync();
            
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Obtém um livro por ID
        /// </summary>
        /// <param name="id">ID do livro</param>
        /// <returns>Livro encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<LivroViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<LivroViewModel>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<LivroViewModel>>> GetById(int id)
        {
            var response = await _livroService.GetByIdAsync(id);
            
            if (!response.Success)
            {
                if (response.Errors.Any(e => e.Contains("não foi encontrado")))
                {
                    return NotFound(response);
                }
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Cria um novo livro
        /// </summary>
        /// <param name="dto">Dados do livro</param>
        /// <returns>Livro criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<LivroViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<LivroViewModel>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<LivroViewModel>>> Create([FromBody] CreateLivroDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<LivroViewModel>
                {
                    Success = false,
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var response = await _livroService.CreateAsync(dto);
            
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetById), new { id = response.Data!.Id, version = "1.0" }, response);
        }

        /// <summary>
        /// Atualiza um livro existente
        /// </summary>
        /// <param name="id">ID do livro</param>
        /// <param name="dto">Dados atualizados do livro</param>
        /// <returns>Livro atualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<LivroViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<LivroViewModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<LivroViewModel>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<LivroViewModel>>> Update(int id, [FromBody] UpdateLivroDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<LivroViewModel>
                {
                    Success = false,
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var response = await _livroService.UpdateAsync(id, dto);
            
            if (!response.Success)
            {
                if (response.Errors.Any(e => e.Contains("não foi encontrado")))
                {
                    return NotFound(response);
                }
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Exclui um livro
        /// </summary>
        /// <param name="id">ID do livro</param>
        /// <returns>Resultado da exclusão</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var response = await _livroService.DeleteAsync(id);
            
            if (!response.Success)
            {
                if (response.Errors.Any(e => e.Contains("não foi encontrado")))
                {
                    return NotFound(response);
                }
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}

