using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PoliceDb.Models
{
    public class ViolazioniVerbali
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid IdVerbale { get; set; }

        [Required]
        public Guid IdViolazione { get; set; }

        [Required(ErrorMessage = "Il campo Importo della violazione è obbligatorio")]
        public decimal Importo { get; set; }

        [Required(ErrorMessage = "Il campo punti decurati della violazione è obbligatorio")]
        public int DecurtamentoPunti { get; set; }

        [ForeignKey("IdVerbale")]
        public Verbali? Verbale { get; set; }

        [ForeignKey("IdViolazione")]
        public TipoViolazione? TipoViolazione { get; set; }
    }
}
