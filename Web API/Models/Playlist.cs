namespace Web_API.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
    }
}
