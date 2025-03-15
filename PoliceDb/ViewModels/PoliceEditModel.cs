using System.ComponentModel.DataAnnotations;
using PoliceDb.Models;

namespace PoliceDb.ViewModels
{
    public class PoliceEditModel
    {
        public Anagrafica? Anagrafica { get; set; }
        public Guid IdVerbale { get; set; }
        public List<ViolazioniVerbali>? Violazioni { get; set; }

        [Required(ErrorMessage = "Il campo Data della violazione è obbligatorio")]
        public DateTime DataViolazione { get; set; }
        [Required(ErrorMessage = "Il campo indirizzo della violazione è obbligatorio")]
        [StringLength(255, ErrorMessage = "Il campo indirizzo della violazione non può superare i 100 caratteri")]
        public string? IndirizzoViolazione { get; set; }
        [Required(ErrorMessage = "Il campo nominativo dell'agente è obbligatorio")]
        [StringLength(255, ErrorMessage = "Il campo nominativo dell'agente non può superare i 100 caratteri")]
        public string? NominativoAgente { get; set; }
        [Required(ErrorMessage = "Il campo Data di trascrizione della violazione è obbligatorio")]
        public DateTime DataTrascrizioneVerbale { get; set; }
        public decimal Importo { get; set; }
        public int DecurtamentoPunti { get; set; }
    }
}
