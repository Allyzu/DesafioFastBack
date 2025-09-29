using DesafioFast.Dto;
using DesafioFast.Models;
using DesafioFast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioFast.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AtasController : ControllerBase
    {
        private readonly IAtaService _ataService;

        public AtasController(IAtaService ataService)
        {
            _ataService = ataService;
        }

        /// <summary>
        /// Cria uma nova ata vinculada a um workshop.
        /// </summary>
        [HttpPost]
        [Route("criar")]

        /// <summary>
        /// Cria uma ata automaticamente com todos os colaboradores do workshop
        /// </summary>
        /// <param name="workshopId">ID do workshop selecionado</param>
        /// <returns>DTO da ata criada</returns>
        [HttpPost("gerar-ata/{workshopId}")]
        public async Task<IActionResult> CriarAtaAutomaticamente(int workshopId)
        {
            var resultado = await _ataService.CriarAtaAutomaticamente(workshopId);

            if (!resultado.Sucesso)
                return BadRequest(new { mensagem = resultado.Mensagem }); // Retorna 400 se já existir

            // Mapear para DTO
            var ataDto = new AtaDto
            {
                Id = resultado.Dados!.Id,
                WorkshopNome = resultado.Dados.Workshop.Nome,
                WorkshopData = resultado.Dados.Workshop.DataRealizacao,
                Colaboradores = resultado.Dados.ColaboradoresList
                    .Select(c => new ColaboradorDto { Id = c.Id, Nome = c.Nome })
                    .ToList()
            };

            return Ok(new { sucesso = true, dados = ataDto });
        }

        //// <summary>
        /// Adiciona um colaborador a uma ata existente.
        /// </summary>
        /// 

        [HttpPut("{ataId}/colaboradores/{colaboradorId}")]
        public async Task<IActionResult> AdicionarColaborador(int ataId, int colaboradorId)
        {
            var resposta = await _ataService.AdicionarColaborador(ataId, colaboradorId);
            if (!resposta.Sucesso)
                return BadRequest(resposta);

            return Ok(resposta);
        }


        /// <summary>
        /// Lista todas as atas.
        /// </summary>

        [HttpGet("listar")]
        public async Task<IActionResult> GetAtas()
        {
            var resposta = await _ataService.GetAtas();
            if (!resposta.Sucesso)
                return BadRequest(resposta);

            return Ok(resposta);
        }

        //// <summary>
        /// Remove um colaborador de uma ata existente.
        /// </summary>
        /// 

        [HttpDelete("{ataId}/colaboradores/{colaboradorId}")]
        public async Task<IActionResult> RemoverColaborador(int ataId, int colaboradorId)
        {
            var resposta = await _ataService.RemoverColaborador(ataId, colaboradorId);

            if (!resposta.Sucesso)
                return BadRequest(new { sucesso = false, mensagem = resposta.Mensagem });

            if (resposta.Dados == null || resposta.Dados.Workshop == null)
                return Ok(new { sucesso = true, mensagem = resposta.Mensagem, dados = new { } });

            var ataDto = new AtaDto
            {
                Id = resposta.Dados.Id,
                WorkshopNome = resposta.Dados.Workshop.Nome,
                WorkshopData = resposta.Dados.Workshop.DataRealizacao,
                Colaboradores = resposta.Dados.ColaboradoresList
                    .Select(c => new ColaboradorDto { Id = c.Id, Nome = c.Nome })
                    .ToList()
            };

            return Ok(new { sucesso = true, mensagem = resposta.Mensagem, dados = ataDto });
        }





        /// <summary>
        /// Obtém uma ata específica, incluindo colaboradores e workshop.
        /// </summary>
        [HttpGet("{ataId}")]
        public async Task<IActionResult> GetAtaId(int ataId)
        {
            var ata = await _ataService.GetAtaById(ataId);

            if (!ata.Sucesso || ata.Dados == null)
                return NotFound(new { Mensagem = ata.Mensagem });

            // Transformar em DTO
            var ataDto = new AtaDto
            {
                Id = ata.Dados.Id,
                WorkshopNome = ata.Dados.Workshop.Nome,
                WorkshopData = ata.Dados.Workshop.DataRealizacao,
                Colaboradores = ata.Dados.ColaboradoresList
                    .Select(c => new ColaboradorDto { Id = c.Id, Nome = c.Nome })
                    .ToList()
            };

            return Ok(new{sucesso = true, mensagem = "Ata encontrada com sucesso.", dados = ataDto});
        }


        [HttpGet("workshop/{workshopId}")]
        public async Task<IActionResult> GetAtaByWorkshop(int workshopId)
        {
            var ata = await _ataService.GetAtaByWorkshopId(workshopId);
            if (!ata.Sucesso || ata.Dados == null)
                return NotFound(new { Mensagem = ata.Mensagem });

            var ataDto = new AtaDto
            {
                Id = ata.Dados.Id,
                WorkshopNome = ata.Dados.Workshop.Nome,
                WorkshopData = ata.Dados.Workshop.DataRealizacao,
                Colaboradores = ata.Dados.ColaboradoresList
                    .Select(c => new ColaboradorDto { Id = c.Id, Nome = c.Nome })
                    .ToList()
            };

            return Ok(new { sucesso = true, dados = ataDto });
        }



    }
}
