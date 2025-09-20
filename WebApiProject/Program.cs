using Microsoft.EntityFrameworkCore;
using WebApiProject.Data;
using WebApiProject.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder => {
    var connectionString = builder.Configuration.GetConnectionString("StudyDB");
    optionsBuilder.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddHttpClient("PersonApi", client => {
    client.BaseAddress = new Uri("http://localhost:5019/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddScoped<IWebApiExecutor, WebApiExecutor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
