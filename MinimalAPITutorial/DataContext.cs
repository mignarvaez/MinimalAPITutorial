global using Microsoft.EntityFrameworkCore; //Se indica que se usara de manera global para que el archivo program.cs también lo incluya

namespace MinimalAPITutorial
{
    /// <summary>
    /// Clase datacontext que permite la interacción con la base de datos
    /// </summary>
    public class DataContext:DbContext 
    {   
        // Constructor
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }

        // Este set representa la tabla de enfermedades que se creara para la entidad enfermedad
        public DbSet<Enfermedad>Enfermedades => Set<Enfermedad>();
    }
}
