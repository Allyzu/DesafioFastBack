namespace DesafioFast.Models
{
    public class WorkshopModels
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataRealizacao { get; set; }
        public string Descricao { get; set; } = string.Empty;

        public ICollection<ColaboradorModels> Colaboradores { get; set; }
        public ICollection<AtaModels> Atas { get; set; }
    }
}
