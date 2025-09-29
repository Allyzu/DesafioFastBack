namespace DesafioFast.Dto
{
    public class AtaDto
    {
        public int Id { get; set; }
        public string WorkshopNome { get; set; }
        public DateTime WorkshopData { get; set; }
        public List<ColaboradorDto> Colaboradores { get; set; } = new();
    }

    public class ColaboradorDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
