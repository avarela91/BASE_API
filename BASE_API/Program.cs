using DAL;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(builder.Configuration); // Registrar IConfiguration como un singleton

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var groupName = "v1";

    options.SwaggerDoc(groupName, new OpenApiInfo
    {
        Title = $"BASE API {groupName}",
        Version = groupName,
        Description = "APIBASE",
        Contact = new OpenApiContact
        {
            Name = "SOFTWARE A&E",
            Email = string.Empty,
            Url = new Uri("https://www.otro.com/"),
        }
    });

});

// services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder => {
    builder.WithOrigins(
        "https://localhost:44358/",
        "http://localhost:44358/"
        )
            .AllowAnyMethod()
            .AllowAnyHeader();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
