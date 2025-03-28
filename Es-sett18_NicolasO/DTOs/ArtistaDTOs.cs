namespace Es_sett18_NicolasO.DTOs
{
    public class ArtistaDto
    {
        public int ArtistaId { get; set; }
        public string Nome { get; set; }
        public string Genere { get; set; }
        public string Biografia { get; set; }
    }

    public class ArtistaCreateDto
    {
        public string Nome { get; set; }
        public string Genere { get; set; }
        public string Biografia { get; set; }
    }
}
