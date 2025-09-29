using DesafioFast.Dto;
using DesafioFast.Models;
using DesafioFast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioFast.Controllers
{
    [ApiController]
    [Route("api/colaboradores")]
    [Authorize] 
    public class ColaboradoresControllers : ControllerBase
    {
        private readonly IColaboradorService _colaboradorService;
        public ColaboradoresControllers(IColaboradorService colaboradorService)
        {
            _colaboradorService = colaboradorService;
        }

        /// <summary>
        /// Cria um novo colaborador
        /// </summary>
        [HttpPost("Criarcolaboradores")]
       
        public async Task<IActionResult> CriarColaborador([FromBody] ColaboradoresCriacaoDto colaboradoresCriacaoDto)
        {
            var resposta = await _colaboradorService.CriarColaborador(colaboradoresCriacaoDto);
            return Ok(resposta); // Sempre 200, o Sucesso indica sucesso ou falha
        }

        [HttpPost("{nome}/workshops/{workshopId}")]
        public async Task<IActionResult> AdicionarColaboradorEmWorkshop(string nome, int workshopId)
        {
            var (sucesso, mensagem) = await _colaboradorService.AdicionarColaboradorEmWorkshop(nome, workshopId);

            if (!sucesso)   
                return BadRequest(new { sucesso, mensagem });

            return Ok(new { sucesso, mensagem });
        }

        /// <summary>
        /// Retorna todos os colaboradores com workshops que participaram
        /// </summary>
        [HttpGet("listarcolaboradores")]
        public async Task<IActionResult> GetTodosColaboradores()
        {
            var colaboradores = await _colaboradorService.GetTodosColaboradores();
            return Ok(colaboradores);
        }

        /// <summary>
        /// Retorna todos os colaboradores em ordem alfabética
        /// com workshops que participaram
        /// </summary>
        [HttpGet("ordenados")]
        public async Task<IActionResult> GetColaboradoresOrdenados()
        {
            var colaboradores = await _colaboradorService.GetColaboradoresOrdenados();
            return Ok(colaboradores);
        }
    }
}
