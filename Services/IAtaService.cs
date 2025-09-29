using DesafioFast.Dto;
using DesafioFast.Models;
using static DesafioFast.Dto.CriarAtaCriacaoDto;

namespace DesafioFast.Services
{
    public interface IAtaService
    {
        Task<ResponseModel<AtaModels?>> CriarAtaAutomaticamente(int workshopId);

        Task<ResponseModel<List<CriarAtaCriacaoDto.AtaCriadaDto>>> GetAtas();
        Task<ResponseModel<AtaModels?>> AdicionarColaborador(int ataId, int colaboradorId);
        Task<ResponseModel<AtaModels?>> RemoverColaborador(int ataId, int colaboradorId);
        Task<ResponseModel<AtaModels?>> GetAtaById(int ataId);

        Task<ResponseModel<AtaModels?>> GetAtaByWorkshopId(int workshopId);
    }
}
