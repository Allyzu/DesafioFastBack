namespace DesafioFast.Models
{
    public class ResponseModel <T>
    {
        public T? Dados  { get; set; }
        public string Mensagem { get; set; }
        public bool Sucesso { get; set; }
    }
}
