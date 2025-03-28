namespace Es_sett18_NicolasO.Models
{
    public class Artista
    {
        public int ArtistaId { get; set; }
        public string Nome { get; set; }
        public string Genere { get; set; }
        public string Biografia { get; set; }

        public virtual ICollection<Evento> Eventi { get; set; }
    }
}
