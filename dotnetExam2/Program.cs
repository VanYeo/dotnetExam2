using dotnetExam2.Endpoints;
using dotnetExam2.Persistence;
using dotnetExam2.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Registering the DbContext with PostgreSQL support

builder.Services.AddDbContext<MovieDbContext>(options =>
{
    // use connection string from appsettings.jso
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

// Allows injection of service into endpoints
builder.Services.AddTransient<IMovieService, MovieService>();

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
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();

app.MapControllers();

// register movie specific endpoints
app.MapMovieEndpoints();

app.MapGet("/", () => "Hello World!")
   .Produces(200, typeof(string));

app.Run();
