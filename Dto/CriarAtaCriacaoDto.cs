namespace DesafioFast.Dto
{
    public class CriarAtaCriacaoDto
    {
        public int WorkshopId { get; set; }
        public List<int> ColaboradoresIds { get; set; } = new List<int>();
        public List<string>? ColaboradoresNomes { get; set; }

        // DTO que será retornado quando uma ata for criada
        public class AtaCriadaDto
        {
            public int Id { get; set; } // Id da ata
            public string WorkshopNome { get; set; } = string.Empty; // Nome do workshop
            public DateTime DataRealizacao { get; set; } // Data do workshop
            public List<ColaboradoresCriacaoDto> Colaboradores { get; set; } = new List<ColaboradoresCriacaoDto>(); // Colaboradores participantes
        }
    }
}
