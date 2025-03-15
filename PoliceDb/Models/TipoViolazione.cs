using System.ComponentModel.DataAnnotations;

namespace PoliceDb.Models
{
    public class TipoViolazione
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Descrizione { get; set; }

    }
}
