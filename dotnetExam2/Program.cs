using dotnetExam2.Persistence;
using dotnetExam2.Repositories;
using Microsoft.EntityFrameworkCore;
using dotnetExam2.Mappings;
using dotnetExam2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registering the DbContext with PostgreSQL support

builder.Services.AddDbContext<MovieDbContext>(options =>
{
    // use connection string from appsettings.jso
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

// Allows injection of service into endpoints
builder.Services.AddScoped<IMovieRepository, SQLMovieRepository>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

// configuring openAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// ensure DB created asynchronously on startup (not recommended for production use)
await using (var serviceScope = app.Services.CreateAsyncScope())
await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<MovieDbContext>())
{
    await dbContext.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();


app.MapGet("/", () => "Hello World!")
   .Produces(200, typeof(string));

app.Run();
