
namespace DesafioFast.Dto
{
    public class WorkshopCriacaoDto
    {
        public List<ColaboradoresCriacaoDto> Colaboradores { get; set; } = new List<ColaboradoresCriacaoDto>();

        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataRealizacao { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
