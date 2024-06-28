using System.ComponentModel.DataAnnotations;

namespace Washyn.UNAJ.Lot.Models
{
    public class DocumentOptions
    {
        [Required]
        public string YearName { get; set; }

        [Required]
        public string NumeroCarta { get; set; }

        [Required]
        public string Asunto { get; set; }

        [Required]
        public string Modalidad { get; set; }

        [Required]
        public int SequenceStart { get; set; }

        [Required]
        public string FechaExamen { get; set; }

        [Required]
        public string Despedida { get; set; }

        [Required]
        public string FechaGenerada { get; set; }
    }
}