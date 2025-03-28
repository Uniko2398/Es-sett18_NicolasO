namespace Es_sett18_NicolasO.DTOs
{
    public class BigliettoDto
    {
        public int BigliettoId { get; set; }
        public DateTime DataAcquisto { get; set; }
        public int EventoId { get; set; }
        public string EventoTitolo { get; set; }
        public DateTime EventoData { get; set; }
        public string EventoLuogo { get; set; }
        public string ArtistaNome { get; set; }
    }

    public class AcquistoBigliettoDto
    {
        public int EventoId { get; set; }
        public int Quantita { get; set; } = 1;
    }
}
