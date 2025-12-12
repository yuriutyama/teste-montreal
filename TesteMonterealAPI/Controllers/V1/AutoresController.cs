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
    public class AutoresController : ControllerBase
    {
        private readonly IAutorService _autorService;

        public AutoresController(IAutorService autorService)
        {
            _autorService = autorService;
        }

        /// <summary>
        /// Obtém todos os autores
        /// </summary>
        /// <returns>Lista de autores</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AutorViewModel>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<AutorViewModel>>>> GetAll()
        {
            var response = await _autorService.GetAllAsync();
            
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Obtém um autor por ID
        /// </summary>
        /// <param name="id">ID do autor</param>
        /// <returns>Autor encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AutorViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AutorViewModel>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<AutorViewModel>>> GetById(int id)
        {
            var response = await _autorService.GetByIdAsync(id);
            
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
        /// Cria um novo autor
        /// </summary>
        /// <param name="dto">Dados do autor</param>
        /// <returns>Autor criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<AutorViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<AutorViewModel>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<AutorViewModel>>> Create([FromBody] CreateAutorDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<AutorViewModel>
                {
                    Success = false,
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var response = await _autorService.CreateAsync(dto);
            
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetById), new { id = response.Data!.Id, version = "1.0" }, response);
        }

        /// <summary>
        /// Atualiza um autor existente
        /// </summary>
        /// <param name="id">ID do autor</param>
        /// <param name="dto">Dados atualizados do autor</param>
        /// <returns>Autor atualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AutorViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AutorViewModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<AutorViewModel>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<AutorViewModel>>> Update(int id, [FromBody] UpdateAutorDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<AutorViewModel>
                {
                    Success = false,
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var response = await _autorService.UpdateAsync(id, dto);
            
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
        /// Exclui um autor
        /// </summary>
        /// <param name="id">ID do autor</param>
        /// <returns>Resultado da exclusão</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var response = await _autorService.DeleteAsync(id);
            
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

