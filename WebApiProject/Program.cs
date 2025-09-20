using Microsoft.EntityFrameworkCore;
using WebApiProject.Data;
using WebApiProject.Models.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder => {
    var connectionString = builder.Configuration.GetConnectionString("StudyDB");
    optionsBuilder.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
