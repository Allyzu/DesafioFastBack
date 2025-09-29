using DesafioFast.Data;
using DesafioFast.Models;
using DesafioFast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DesafioFast.Dto;

namespace DesafioFast.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkshopsController : ControllerBase
    {
        private readonly IWorkshopService _workshopService;

        public WorkshopsController(IWorkshopService workshopService)
        {
            _workshopService = workshopService;
        }


        /// <summary>
        /// Cria um novo workshop
        /// </summary>
        [HttpPost("criar")]
        public async Task<IActionResult> CriarWorkshop([FromBody] WorkshopDto workshopdto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resposta = await _workshopService.CriarWorkshop(workshopdto);

            if (!resposta.Sucesso)
                return BadRequest(resposta);

            return Ok(resposta);
        }

        /// <summary>
        /// 📋 Listar todos os workshops (com nome, data e descrição)
        /// </summary>
        [HttpGet("GetAllWorkshops")]
        public async Task<IActionResult> ListarWorkshops()
        {
            var resposta = await _workshopService.ListarWorkshops();

            if (!resposta.Sucesso)
                return BadRequest(resposta);

            return Ok(resposta);
        }

        /// <summary>
        /// 🔍 Buscar workshops por nome
        /// </summary>
        [HttpGet("buscar-nome")]
        public async Task<IActionResult> BuscarPorNome([FromQuery] string nome)
        {
            var workshops = await _workshopService.WorkshopsByNome(nome);
            return Ok(workshops);
        }

        /// <summary>
        /// 📅 Buscar workshops por data
        /// </summary>
        [HttpGet("buscar-data")]
        public async Task<IActionResult> BuscarPorData([FromQuery] DateTime data)
        {
            var workshops = await _workshopService.WorkshopsByData(data);
            return Ok(workshops);
        }

        /// <summary>
        /// 👥 Buscar colaboradores de um workshop
        /// </summary>
        [HttpGet("{workshopId}/colaboradores")]
        public async Task<IActionResult> GetWorkshopColaboradores(int workshopId)
        {
            var colaboradores = await _workshopService.WorkshopsByColaboradores(workshopId);

            if (colaboradores == null)
                return NotFound("Workshop não encontrado.");

            return Ok(colaboradores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var workshop = await _workshopService.GetWorkshopById(id); // Crie esse método
            if (workshop == null)
                return NotFound();
            return Ok(workshop);
        }
    }
}
