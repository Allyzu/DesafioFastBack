namespace DesafioFast.Models
{
    public class ColaboradorModels
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public ICollection<WorkshopModels> Workshops { get; set; } 
        public ICollection<AtaModels> Atas { get; set; }
    }
}
