using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registro do contexto como um servi�o (inje��o de dependencia)
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TarefasDB"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Ol� Mundo!");

//definido a rota do end point
app.MapGet("/tarefas", async (AppDbContext db) => await db.Tarefas.ToListAsync());

app.MapPost("/tarefas", async (Tarefa tarefa, AppDbContext db) =>
{
    db.Tarefas.Add(tarefa);
    await db.SaveChangesAsync();
    return Results.Created($"/tarefas/{tarefa.Id}", tarefa);

});



app.Run();

//definido modelo de dom�nio
public class Tarefa    
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public bool IsConcuida { get; set; }
}

//definida a sess�o com os objetos em m�moria!  (classe que coordena o EF) 
class AppDbContext : DbContext  
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
}
