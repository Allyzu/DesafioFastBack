namespace DesafioFast.Dto
{

    public class ColaboradoresCriacaoDto
    {
        public List<WorkshopCriacaoDto> Workshops { get; set; } = new List<WorkshopCriacaoDto>();
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int WorkshopId { get; set; }
    }

    public class ColaboradorRetornoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public WorkshopSimplesDto Workshop { get; set; } = new WorkshopSimplesDto();
    }

    public class WorkshopSimplesDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }

}
