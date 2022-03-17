namespace MinimalAPITutorial
{
    public class Enfermedad
    {
        public int Id { get; set; } 
        public string Nombre { get; set; } = string.Empty;
        public string CodigoCIE10 { get; set; } = string.Empty;
        public string Duracion { get; set; } = string.Empty;
        public string Distribucion { get; set; } = string.Empty;
        public string Etiopatogenia { get; set; } = string.Empty;

    }
}
