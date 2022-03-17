using MinimalAPITutorial;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Se agrega a la api el data context
// Se especifican las opciones
// Para crear la migraci�n: dotnet ef migrations add Initial (Se corre desde la consola del administrador de paquetes)
// y estando en la carpeta del proyecto
// Para correr la migraci�n: dotnet ef database update
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")) // Se indica la cadena de conexi�n
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/// <summary>
/// Funci�n que retorna todas la enfermedades
/// </summary>
async Task <List<Enfermedad>> GetAllEnfermedades(DataContext context) => 
    await context.Enfermedades.ToListAsync();

app.MapGet("/", () => "Bienvenido a la base de datos de enfermedades");

/// <summary>
/// M�todo que devuelve todas las enfermedades almacenadas. 
/// Se especifica la ruta, y el m�todo aysnc con el contexto y se obtiene la lista de las enfermedades
/// </summary>
app.MapGet("/enfermedad", async (DataContext context) =>
    await context.Enfermedades.ToListAsync()
    );

/// <summary>
/// M�todo que retorna una enfermedad seg�n su id
/// Busca de manera asincrona y en caso de que encuentre la enfermedad retorna un mensaje de tipo 200 con la enfermedad encontrada
/// En caso contrario retorna un mensaje 404.
/// </summary>
app.MapGet("/enfermedad/{id}", async (DataContext context, int id) =>
    await context.Enfermedades.FindAsync(id) is Enfermedad enfermedad ?         
        Results.Ok(enfermedad):
        Results.NotFound("Lo sentimos, no se encontr� una enfermedad con el id suministrado")
        );

/// <summary>
/// M�todo que crea una enfermedad
/// Agrega la enfermedad al context y la guarda de manera asincrona
/// </summary>
app.MapPost("/enfermedad", async (DataContext context, Enfermedad enfermedad) =>
{
        context.Enfermedades.Add(enfermedad);
        await context.SaveChangesAsync();
        // Retorna todas las enfermedades del sistema
        return Results.Ok(await GetAllEnfermedades(context));
});

/// <summary>
/// M�todo que actualiza una enfermedad
/// </summary>
app.MapPut("/enfermedad/{id}", async(DataContext context, Enfermedad enfermedad, int id) =>
{   
    //Busca la enfermedad, si no la encuentra muestra mensaje infomando la situaci�n
    var dbEnfermedad = await context.Enfermedades.FindAsync(id);
    if (dbEnfermedad == null) return Results.NotFound("No se encontr� una enfermedad con el id suministrado. :/");

    // A la enfermedad encontrada se le asignan los nuevos valores, se almacena de manera asincrona y se retorna la lista de las enfermedades.
    dbEnfermedad.Nombre = enfermedad.Nombre;
    dbEnfermedad.CodigoCIE10 = enfermedad.CodigoCIE10;
    dbEnfermedad.Duracion = enfermedad.Duracion;
    dbEnfermedad.Distribucion = enfermedad.Distribucion;
    dbEnfermedad.Etiopatogenia = enfermedad.Etiopatogenia;
    
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllEnfermedades(context));
});

/// <summary>
/// M�todo que elimina una enfermedad, si encuentra una enfermedad la elimina, en caso contrario muestra un mensaje informando la situaci�n.
/// </summary>
app.MapDelete("/enfermedad/{id}", async (DataContext context, int id) =>
{
    var dbEnfermedad = await context.Enfermedades.FindAsync(id);
    if (dbEnfermedad == null) return Results.NotFound("No se encontr� una enfermedad con el id suministrado. :/");

    context.Enfermedades.Remove(dbEnfermedad);
    await context.SaveChangesAsync();

    return Results.Ok(await GetAllEnfermedades(context));

});

app.Run();
