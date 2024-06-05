namespace Washyn.UNAJ.Lot.Models
{
    public class DocumentModel
    {
        public string YearName { get; set; }
        public DateTime DateGenerated { get; set; }
        public string NumberLetter { get; set; }
        public string DegreeLevel { get; set; }
        public string FullName { get; set; }
        public string Asunto { get; set; }
        public string TestName { get; set; }
        public string RolName { get; set; }
    }

    // add this settings via appsettings.json
    public class DocumentOptions
    {
        public string YearName { get; set; }
        public string NumeroCarta { get; set; }
        public string Asunto { get; set; }
        public string NombrePrueba { get; set; }
        public string Modalidad { get; set; }
        public DateTime FechaGenerada { get; set; } // TODO: mapp en app settiings.
    }
}