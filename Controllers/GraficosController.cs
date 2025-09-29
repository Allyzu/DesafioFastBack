using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioFast.Data; // ajuste conforme seu DbContext
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/graficos")]
public class GraficosController : ControllerBase
{
    private readonly ApppDbContext _context;

    public GraficosController(ApppDbContext context)
    {
        _context = context;
    }

    // Gráfico de barras: quantidade de workshops por colaborador
    [HttpGet("workshops-por-colaborador")]
    public async Task<IActionResult> WorkshopsPorColaborador()
    {
        var dados = await _context.Colaboradores
            .Select(c => new
            {
                Nome = c.Nome,
                TotalWorkshops = c.Workshops.Count
            })
            .ToListAsync();

        return Ok(dados);
    }

    // Gráfico de pizza: quantidade de colaboradores por workshop
    [HttpGet("colaboradores-por-workshop")]
    public async Task<IActionResult> ColaboradoresPorWorkshop()
    {
        var dados = await _context.Workshops
            .Select(w => new
            {
                Nome = w.Nome,
                TotalColaboradores = w.Colaboradores.Count
            })
            .ToListAsync();

        return Ok(dados);
    }
}
