using DesafioFast.Data;
using DesafioFast.Dto;
using DesafioFast.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioFast.Services
{
    public class ColaboradorService : IColaboradorService
    {
        private readonly ApppDbContext _context;
        public ColaboradorService(ApppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os colaboradores
        /// junto com os workshops que eles participaram mais informações
        /// /// </summary>
        public async Task<List<ColaboradoresCriacaoDto>> GetTodosColaboradores()
        {
            return await _context.Colaboradores
                .Include(c => c.Workshops)
                .Select(c => new ColaboradoresCriacaoDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Workshops = c.Workshops
                        .Select(w => new WorkshopCriacaoDto
                        {
                            Id = w.Id,
                            Nome = w.Nome,
                            DataRealizacao = w.DataRealizacao,
                            Descricao = w.Descricao
                        }).ToList()
                })
                .ToListAsync();
        }

        /// <summary>
        /// Cria um novo colaborador
        /// /// </summary>
        public async Task<ResponseModel<ColaboradorRetornoDto>> CriarColaborador(ColaboradoresCriacaoDto colaboradoresCriacaoDto)
        {
            var resposta = new ResponseModel<ColaboradorRetornoDto>();

            try
            {
                var novoColaborador = new ColaboradorModels
                {
                    Nome = colaboradoresCriacaoDto.Nome
                };

                // Busca o workshop selecionado
                var workshop = await _context.Workshops.FindAsync(colaboradoresCriacaoDto.WorkshopId);
                if (workshop == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Workshop não encontrado.";
                    resposta.Dados = null;
                    return resposta;
                }

                // Adiciona o colaborador ao workshop
                novoColaborador.Workshops = new List<WorkshopModels> { workshop };
                _context.Colaboradores.Add(novoColaborador);
                await _context.SaveChangesAsync();

                // Atualiza a ata automaticamente, se existir
                var ata = await _context.Atas
                    .Include(a => a.ColaboradoresList)
                    .FirstOrDefaultAsync(a => a.WorkshopId == workshop.Id);

                if (ata != null && !ata.ColaboradoresList.Any(c => c.Id == novoColaborador.Id))
                {
                    ata.ColaboradoresList.Add(novoColaborador);
                    await _context.SaveChangesAsync();
                }

                // Retorna apenas os dados necessários via DTO
                resposta.Dados = new ColaboradorRetornoDto
                {
                    Id = novoColaborador.Id,
                    Nome = novoColaborador.Nome,
                    Workshop = new WorkshopSimplesDto
                    {
                        Id = workshop.Id,
                        Nome = workshop.Nome
                    }
                };

                resposta.Sucesso = true;
                resposta.Mensagem = "Colaborador criado com sucesso.";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Mensagem = ex.Message;
                resposta.Dados = null;
                return resposta;
            }
        }


        public async Task<(bool sucesso, string mensagem)> AdicionarColaboradorEmWorkshop(string nome, int workshopId)
        {
            var colaborador = await _context.Colaboradores.FirstOrDefaultAsync(c => c.Nome == nome);
            if (colaborador == null)
                return (false, "Colaborador não existe.");

            var workshop = await _context.Workshops.Include(w => w.Colaboradores).FirstOrDefaultAsync(w => w.Id == workshopId);
            if (workshop == null)
                return (false, "Workshop não encontrado.");

            if (workshop.Colaboradores.Any(c => c.Id == colaborador.Id))
                return (false, "Colaborador já está neste workshop.");

            workshop.Colaboradores.Add(colaborador);
            await _context.SaveChangesAsync();

            // Atualizar a ata automaticamente, se existir
            var ata = await _context.Atas.Include(a => a.ColaboradoresList).FirstOrDefaultAsync(a => a.WorkshopId == workshopId);
            if (ata != null && !ata.ColaboradoresList.Any(c => c.Id == colaborador.Id))
            {
                ata.ColaboradoresList.Add(colaborador);
                await _context.SaveChangesAsync();
            }

            return (true, "Colaborador adicionado com sucesso.");
        }


        /// <summary>
        /// Retorna todos os colaboradores em ordem alfabética
        /// junto com os workshops que eles participaram mais informações
        /// </summary>
        public async Task<List<ColaboradoresCriacaoDto>> GetColaboradoresOrdenados()
        {
            return await _context.Colaboradores
                .Include(c => c.Workshops)
                .OrderBy(c => c.Nome) // ordena alfabeticamente
                .Select(c => new ColaboradoresCriacaoDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Workshops = c.Workshops
                        .Select(w => new WorkshopCriacaoDto
                        {
                            Id = w.Id,
                            Nome = w.Nome,
                            DataRealizacao = w.DataRealizacao,
                            Descricao = w.Descricao
                        }).ToList()
                })
                .ToListAsync();
        }


    }


}
