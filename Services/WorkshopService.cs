using DesafioFast.Data;
using DesafioFast.Dto;
using DesafioFast.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DesafioFast.Services;



public class WorkshopService : IWorkshopService
{
    private readonly ApppDbContext _context;
    public WorkshopService(ApppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Faz a criação de um novo workshop
    /// </summary>
    public async Task<ResponseModel<List<WorkshopModels>>> CriarWorkshop([FromBody] WorkshopDto workshop)
    {
        ResponseModel<List<WorkshopModels>> resposta = new ResponseModel<List<WorkshopModels>>();

        try
        {
            var novoWorkshop = new WorkshopModels
            {
                Nome = workshop.Nome,
                DataRealizacao = workshop.DataRealizacao,
                Descricao = workshop.Descricao
            };
            _context.Workshops.Add(novoWorkshop);
            await _context.SaveChangesAsync();
            var workshops = await _context.Workshops.ToListAsync();
            resposta.Dados = workshops;
            resposta.Mensagem = "Workshop criado com sucesso.";
            resposta.Sucesso = true;
            return resposta;
        }
        catch (Exception ex)
        {
            resposta.Mensagem = ex.Message;
            resposta.Sucesso = false;
            return resposta;
        }
    }

    /// <summary>
    /// Faz a listagem de todos os workshops
    /// Retorna apenas os worshops (sem colaboradores ainda)
    /// </summary>
    public async Task<ResponseModel<List<WorkshopCriacaoDto>>> ListarWorkshops()
    {
        var resposta = new ResponseModel<List<WorkshopCriacaoDto>>();
        try
        {
            var workshops = await _context.Workshops
                .Select(w => new WorkshopCriacaoDto
                {
                    Id = w.Id,
                    Nome = w.Nome,
                    DataRealizacao = w.DataRealizacao,
                    Descricao = w.Descricao
                })
                .ToListAsync();

            resposta.Dados = workshops;
            resposta.Mensagem = "Workshops listados com sucesso.";
            resposta.Sucesso = true;
        }
        catch (Exception ex)
        {
            resposta.Mensagem = ex.Message;
            resposta.Sucesso = false;
            return resposta;
        }

        return resposta;
    }

    /// <summary>
    /// 👥 Buscar todos os colaboradores de um workshop selecionado
    /// Retorna o nome do workshop + lista de colaboradores
    /// </summary>
    public async Task<object?> WorkshopsByColaboradores(int workshopId)
    {
        var workshop = await _context.Workshops
            .Include(w => w.Colaboradores)
            .FirstOrDefaultAsync(w => w.Id == workshopId);

        if (workshop == null) return null;

        var result = new
        {
            Workshop = workshop.Nome,
            Data = workshop.DataRealizacao,
            Colaboradores = workshop.Colaboradores.Select(c => new
            {
                c.Id,
                c.Nome
            }).OrderBy(c => c.Nome).ToList()
        };

        return result;
    }

    /// <summary>
    /// 🔍 Buscar todos os workshops que contenham parte do nome pesquisado
    /// Retorna apenas informações dos workshops (sem colaboradores ainda)
    /// </summary>
  
    public async Task<List<WorkshopCriacaoDto>> WorkshopsByNome(string nome)
    {
        return await _context.Workshops
            .Where(w => w.Nome.Contains(nome))
            .Include(w => w.Colaboradores)
            .Select(w => new WorkshopCriacaoDto
            {
                Id = w.Id,
                Nome = w.Nome,
                DataRealizacao = w.DataRealizacao,
                Descricao = w.Descricao,
                Colaboradores = w.Colaboradores
                    .Select(c => new ColaboradoresCriacaoDto
                    {
                        Id = c.Id,
                        Nome = c.Nome
                    }).ToList()
            })
            .ToListAsync();
    }

    /// <summary>
    /// 🔍 Buscar todos os workshops de uma data específica
    /// Retorna apenas informações dos workshops (sem colaboradores ainda)
    /// </summary>
    public async Task<List<WorkshopCriacaoDto>> WorkshopsByData(DateTime data)
    {
        var startDate = data.Date; // início do dia
        var endDate = data.Date.AddDays(1); // início do próximo dia

        var workshops = await _context.Workshops
            .Where(w => w.DataRealizacao >= startDate && w.DataRealizacao < endDate)
            .Include(w => w.Colaboradores)
            .Select(w => new WorkshopCriacaoDto
            {
                Id = w.Id,
                Nome = w.Nome,
                DataRealizacao = w.DataRealizacao,
                Descricao = w.Descricao,
                Colaboradores = w.Colaboradores
                    .Select(c => new ColaboradoresCriacaoDto
                    {
                        Id = c.Id,
                        Nome = c.Nome
                    }).ToList()
            })
            .ToListAsync();

        return workshops;
    }

    public async Task<WorkshopCriacaoDto?> GetWorkshopById(int id)
    {
        return await _context.Workshops
            .Where(w => w.Id == id)
            .Include(w => w.Colaboradores)
            .Select(w => new WorkshopCriacaoDto
            {
                Id = w.Id,
                Nome = w.Nome,
                DataRealizacao = w.DataRealizacao,
                Descricao = w.Descricao,
                Colaboradores = w.Colaboradores
                    .Select(c => new ColaboradoresCriacaoDto
                    {
                        Id = c.Id,
                        Nome = c.Nome
                    }).ToList()
            }).FirstOrDefaultAsync();
    }

}
