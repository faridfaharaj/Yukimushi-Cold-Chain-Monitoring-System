using Api;
using Data;
using Readings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DbConnectionFactory>();
builder.Services.AddScoped<IReadingRepository, ReadingRepository>();
builder.Services.AddScoped<IReadingService, ReadingService>();

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.MapRazorPages();

app.Run();
