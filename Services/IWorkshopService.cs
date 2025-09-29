using DesafioFast.Models;
using DesafioFast.Dto;

namespace DesafioFast.Services
{
    public interface IWorkshopService
    {
        Task<WorkshopCriacaoDto?> GetWorkshopById(int id);
        Task<ResponseModel<List<WorkshopCriacaoDto>>> ListarWorkshops();
        Task<ResponseModel<List<WorkshopModels>>> CriarWorkshop(WorkshopDto workshop);
        Task<List<WorkshopCriacaoDto>> WorkshopsByNome(string nome);    
        Task<List<WorkshopCriacaoDto>> WorkshopsByData(DateTime data);
        Task<object?> WorkshopsByColaboradores(int workshopId);

    }
}
