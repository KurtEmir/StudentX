using Microsoft.EntityFrameworkCore;
using StudentX.StudentXDomain.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("MsSQL");

// DbContext'i servislere ekle
builder.Services.AddDbContext<StudentXDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//api controllerlarını aktive eder
app.MapControllers();


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}



//Chatgpt nin çözümüne bak https://chatgpt.com/c/6804ee35-3960-800b-a440-14e805222cff