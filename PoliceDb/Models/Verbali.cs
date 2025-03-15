using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoliceDb.Models
{
    public class Verbali
    {
        [Key]
        public Guid IdVerbale { get; set; }
        [Required]
        public Guid IdAnagrafica { get; set; }

        [Required]
        public DateTime DataViolazione { get; set; }

        [Required(ErrorMessage = "Il campo indirizzo della violazione è obbligatorio")]
        [StringLength(255, ErrorMessage = "Il campo indirizzo della violazione non può superare i 100 caratteri")]
        public string? IndirizzoViolazione { get; set; }

        [Required(ErrorMessage = "Il campo nominativo dell'agente è obbligatorio")]
        [StringLength(255, ErrorMessage = "Il campo nominativo dell'agente non può superare i 100 caratteri")]
        public string? NominativoAgente { get; set; }

        [Required(ErrorMessage = "Il campo Data di trascrizione della violazione è obbligatorio")]
        public DateTime DataTrascrizioneVerbale { get; set; }

        [Required(ErrorMessage = "Il campo Importo della violazione è obbligatorio")]
        public decimal TotaleImporto { get; set; }

        [Required(ErrorMessage = "Il campo punti decurati della violazione è obbligatorio")]
        public int TotaleDecurtamentoPunti { get; set; }

        [ForeignKey(nameof(IdAnagrafica))]
        public Anagrafica? Anagrafica { get; set; }
        public ICollection<ViolazioniVerbali> ViolazioniVerbali { get; set; } = new List<ViolazioniVerbali>();

        [NotMapped]
        public int Total { get; set; }

        [NotMapped]
        public decimal Total2 { get; set; }

        [Required]
        public DateTime ScadenzaContestazione { get; set; }
    }
}
