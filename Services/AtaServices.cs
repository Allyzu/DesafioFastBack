using DesafioFast.Data;
using DesafioFast.Dto;
using DesafioFast.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static DesafioFast.Dto.CriarAtaCriacaoDto;

namespace DesafioFast.Services
{
    public class AtaServices : IAtaService
    {
        private readonly ApppDbContext _context;

        public AtaServices(ApppDbContext context)
        {
            _context = context;
        }

        // Cria uma nova ata
        public async Task<ResponseModel<AtaModels?>> CriarAtaAutomaticamente(int workshopId)
        {
            var resposta = new ResponseModel<AtaModels?>();

            try
            {
                // Verificar se já existe uma ata para esse workshop
                var ataExistente = await _context.Atas
                    .FirstOrDefaultAsync(a => a.WorkshopId == workshopId);

                if (ataExistente != null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Já existe uma ata criada para este workshop.";
                    resposta.Dados = null;
                    return resposta;
                }

                // Lógica existente para criar a ata
                var workshop = await _context.Workshops
                    .Include(w => w.Colaboradores)
                    .FirstOrDefaultAsync(w => w.Id == workshopId);

                if (workshop == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Workshop não encontrado.";
                    return resposta;
                }

                var novaAta = new AtaModels
                {
                    WorkshopId = workshop.Id,
                    Workshop = workshop,
                    ColaboradoresList = workshop.Colaboradores.ToList()
                };

                _context.Atas.Add(novaAta);
                await _context.SaveChangesAsync();

                resposta.Sucesso = true;
                resposta.Mensagem = "Ata criada com sucesso.";
                resposta.Dados = novaAta;

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


        public async Task<ResponseModel<List<CriarAtaCriacaoDto.AtaCriadaDto>>> GetAtas()
        {
            var resposta = new ResponseModel<List<CriarAtaCriacaoDto.AtaCriadaDto>>();

            try
            {
                // 1. Busca todas as atas com workshop e colaboradores
                var atas = await _context.Atas
                    .Include(a => a.Workshop)
                    .Include(a => a.ColaboradoresList)
                    .ToListAsync();

                // ------------------------------
                // Mudança 1: se não houver atas, retorna mensagem específica
                // ------------------------------
                if (atas == null || !atas.Any())
                {
                    resposta.Dados = new List<CriarAtaCriacaoDto.AtaCriadaDto>();
                    resposta.Mensagem = "Nenhuma ata criada.";
                    resposta.Sucesso = true;
                    return resposta;
                }

                // ------------------------------
                // Mudança 2: converte AtaModels para AtaCriadaDto
                // Antes: atribuía diretamente 'atas' e dava erro de conversão
                // ------------------------------
                var atasDto = atas.Select(a => new CriarAtaCriacaoDto.AtaCriadaDto
                {
                    Id = a.Id,
                    WorkshopNome = a.Workshop.Nome,
                    DataRealizacao = a.Workshop.DataRealizacao,
                    Colaboradores = a.ColaboradoresList.Select(c => new ColaboradoresCriacaoDto
                    {
                        Id = c.Id,
                        Nome = c.Nome
                    }).ToList()
                }).ToList();

                // ------------------------------
                // Mudança 3: agora atribui 'atasDto' que é do tipo correto
                // ------------------------------
                resposta.Dados = atasDto;
                resposta.Mensagem = "Atas listadas com sucesso.";
                resposta.Sucesso = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Dados = null;
                resposta.Mensagem = ex.Message;
                resposta.Sucesso = false;
                return resposta;
            }
        }

        // Adiciona um colaborador a uma ata existente
        public async Task<ResponseModel<AtaModels?>> AdicionarColaborador(int ataId, int colaboradorId)
        {
            var resposta = new ResponseModel<AtaModels?>();

            try
            {
                var ata = await _context.Atas
                    .Include(a => a.ColaboradoresList)
                    .FirstOrDefaultAsync(a => a.Id == ataId);

                if (ata == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Ata não encontrada.";
                    resposta.Dados = null;
                    return resposta;
                }

                var colaborador = await _context.Colaboradores.FindAsync(colaboradorId);
                if (colaborador == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Colaborador não encontrado.";
                    resposta.Dados = null;
                    return resposta;
                }

                // Verifica se já está adicionado
                if (!ata.ColaboradoresList.Any(c => c.Id == colaboradorId))
                {
                    ata.ColaboradoresList.Add(colaborador);
                    await _context.SaveChangesAsync();
                }

                resposta.Sucesso = true;
                resposta.Mensagem = "Colaborador adicionado com sucesso.";
                resposta.Dados = ata;
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

        // Remover colaborador de uma ata
        public async Task<ResponseModel<AtaModels?>> RemoverColaborador(int ataId, int colaboradorId)
        {
            var resposta = new ResponseModel<AtaModels?>();

            var ata = await _context.Atas
                .Include(a => a.ColaboradoresList)
                .FirstOrDefaultAsync(a => a.Id == ataId);

            if (ata == null)
            {
                resposta.Sucesso = false;
                resposta.Mensagem = "Ata não encontrada.";
                return resposta;
            }

            var colaborador = ata.ColaboradoresList.FirstOrDefault(c => c.Id == colaboradorId);
            if (colaborador == null)
            {
                resposta.Sucesso = false;
                resposta.Mensagem = "Colaborador não encontrado na ata.";
                resposta.Dados = ata;
                return resposta;
            }

            ata.ColaboradoresList.Remove(colaborador);
            await _context.SaveChangesAsync();

            resposta.Sucesso = true;
            resposta.Mensagem = "Colaborador removido com sucesso.";
            resposta.Dados = ata;
            return resposta;
        }


        // Buscar ata por ID, incluindo colaboradores e workshop
        public async Task<ResponseModel<AtaModels?>> GetAtaById(int ataId)
        {
            var resposta = new ResponseModel<AtaModels?>();

            try
            {
                // Busca a ata no banco, incluindo os dados do Workshop e da lista de colaboradores
                var ata = await _context.Atas
                    .Include(a => a.Workshop)
                    .Include(a => a.ColaboradoresList)
                    .FirstOrDefaultAsync(a => a.Id == ataId);

                // Se não encontrar a ata
                if (ata == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Ata não encontrada.";
                    resposta.Dados = null;
                    return resposta;
                }

                // Se encontrar, retorna os dados da ata dentro do ResponseModel
                resposta.Sucesso = true;
                resposta.Mensagem = "Ata encontrada com sucesso.";
                resposta.Dados = ata;
                return resposta;
            }
            catch (Exception ex)
            {
                // Em caso de erro, retorna a mensagem de erro dentro do ResponseModel
                resposta.Sucesso = false;
                resposta.Mensagem = ex.Message;
                resposta.Dados = null;
                return resposta;
            }
        }


        public async Task<ResponseModel<AtaModels?>> GetAtaByWorkshopId(int workshopId)
        {
            var resposta = new ResponseModel<AtaModels?>();
            var ata = await _context.Atas
                .Include(a => a.Workshop)
                .Include(a => a.ColaboradoresList)
                .FirstOrDefaultAsync(a => a.WorkshopId == workshopId);

            if (ata == null)
            {
                resposta.Sucesso = false;
                resposta.Mensagem = "Ata não encontrada para este workshop.";
                resposta.Dados = null;
                return resposta;
            }

            resposta.Sucesso = true;
            resposta.Mensagem = "Ata encontrada com sucesso.";
            resposta.Dados = ata;
            return resposta;
        }



    }
}

