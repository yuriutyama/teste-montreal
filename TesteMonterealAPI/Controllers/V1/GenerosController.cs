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
    public class GenerosController : ControllerBase
    {
        private readonly IGeneroService _generoService;

        public GenerosController(IGeneroService generoService)
        {
            _generoService = generoService;
        }

        /// <summary>
        /// Obtém todos os gêneros
        /// </summary>
        /// <returns>Lista de gêneros</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GeneroViewModel>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<GeneroViewModel>>>> GetAll()
        {
            var response = await _generoService.GetAllAsync();
            
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Obtém um gênero por ID
        /// </summary>
        /// <param name="id">ID do gênero</param>
        /// <returns>Gênero encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<GeneroViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<GeneroViewModel>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<GeneroViewModel>>> GetById(int id)
        {
            var response = await _generoService.GetByIdAsync(id);
            
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
        /// Cria um novo gênero
        /// </summary>
        /// <param name="dto">Dados do gênero</param>
        /// <returns>Gênero criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<GeneroViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<GeneroViewModel>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<GeneroViewModel>>> Create([FromBody] CreateGeneroDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<GeneroViewModel>
                {
                    Success = false,
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var response = await _generoService.CreateAsync(dto);
            
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetById), new { id = response.Data!.Id, version = "1.0" }, response);
        }

        /// <summary>
        /// Atualiza um gênero existente
        /// </summary>
        /// <param name="id">ID do gênero</param>
        /// <param name="dto">Dados atualizados do gênero</param>
        /// <returns>Gênero atualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<GeneroViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<GeneroViewModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<GeneroViewModel>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<GeneroViewModel>>> Update(int id, [FromBody] UpdateGeneroDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<GeneroViewModel>
                {
                    Success = false,
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var response = await _generoService.UpdateAsync(id, dto);
            
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
        /// Exclui um gênero
        /// </summary>
        /// <param name="id">ID do gênero</param>
        /// <returns>Resultado da exclusão</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var response = await _generoService.DeleteAsync(id);
            
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

