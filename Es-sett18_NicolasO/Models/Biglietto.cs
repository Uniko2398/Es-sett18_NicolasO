namespace Es_sett18_NicolasO.Models
{
    public class Biglietto
    {
        public int BigliettoId { get; set; }
        public DateTime DataAcquisto { get; set; }

        public int EventoId { get; set; }
        public virtual Evento Evento { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
