using System.ComponentModel.DataAnnotations;

namespace PoliceDb.Models
{
    public class Anagrafica
    {
        [Key]
        public Guid IdAnagrafica { get; set; }

        [Required(ErrorMessage = "Il campo nome è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il campo nome non può superare i 100 caratteri")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Il campo cognome è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il campo cognome non può superare i 100 caratteri")]
        public string? Cognome { get; set; }

        [Required(ErrorMessage = "Il campo indirizzo è obbligatorio")]
        [StringLength(255, ErrorMessage = "Il campo indirizzo non può superare i 100 caratteri")]
        public string? Indirizzo { get; set; }

        [Required(ErrorMessage = "Il campo città è obbligatorio")]
        [StringLength(255, ErrorMessage = "Il campo città non può superare i 100 caratteri")]
        public string? Città { get; set; }

        [Required(ErrorMessage = "Il campo CAP è obbligatorio")]
        [StringLength(10, ErrorMessage = "Il campo CAP non può superare i 10 caratteri")]
        [RegularExpression(@"^\d{4,10}$", ErrorMessage = "Il campo CAP deve contenere solo cifre e avere una lunghezza compresa tra 4 e 10 caratteri")]
        public string? CAP { get; set; }

        [Required(ErrorMessage = "Il campo Codice Fiscale è obbligatorio")]
        [StringLength(16, ErrorMessage = "Il campo Codice Fiscale deve essere di 16 caratteri")]
        [RegularExpression(@"^[A-Za-z]{6}[0-9]{2}[A-Za-z]{1}[0-9]{2}[A-Za-z]{1}[0-9]{3}[A-Za-z]{1}$",
            ErrorMessage = "Il campo Codice Fiscale non è nel formato corretto")]
        public string? CF { get; set; }

        public ICollection<Verbali> Verbali { get; set; } = new List<Verbali>();
    }
}
