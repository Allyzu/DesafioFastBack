namespace DesafioFast.Models
{
    public class AtaModels
    {
        public int Id { get; set; }
        public int WorkshopId { get; set; }
        public  WorkshopModels Workshop { get; set; }
        public List<ColaboradorModels> ColaboradoresList { get; set; }

    }
}

