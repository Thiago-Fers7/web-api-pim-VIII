namespace Web_API.Models
{
    public class Conteudo
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int CriadorId { get; set; }
    }

    public class ConteudoResponse : Conteudo
    {
        public int PlaylistsCount { get; set; }
    }
}
