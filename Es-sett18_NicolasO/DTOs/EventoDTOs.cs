﻿namespace Es_sett18_NicolasO.DTOs
{
    public class EventoDto
    {
        public int EventoId { get; set; }
        public string Titolo { get; set; }
        public DateTime Data { get; set; }
        public string Luogo { get; set; }
        public int ArtistaId { get; set; }
        public string ArtistaNome { get; set; }
    }

    public class EventoCreateDto
    {
        public string Titolo { get; set; }
        public DateTime Data { get; set; }
        public string Luogo { get; set; }
        public int ArtistaId { get; set; }
    }

}
