using DesafioFast.Dto;
using DesafioFast.Models;
using System.Threading.Tasks;

namespace DesafioFast.Services
{
    public interface IColaboradorService
    {
        Task<ResponseModel<ColaboradorRetornoDto>> CriarColaborador(ColaboradoresCriacaoDto colaboradoresCriacaoDto);
        Task<(bool sucesso, string mensagem)> AdicionarColaboradorEmWorkshop(string nome, int workshopId);
        Task<List<ColaboradoresCriacaoDto>> GetTodosColaboradores();
        Task<List<ColaboradoresCriacaoDto>> GetColaboradoresOrdenados();
    }
}
