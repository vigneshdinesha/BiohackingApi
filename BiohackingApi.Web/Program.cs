using Microsoft.EntityFrameworkCore;
using BiohackingApi.Web.Data;
using BiohackingApi.Web.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configure JSON serialization for camelCase
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Add Entity Framework
builder.Services.AddDbContext<BiohackingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories
builder.Services.AddScoped<IGenericRepository<BiohackingApi.Web.Models.User>, GenericRepository<BiohackingApi.Web.Models.User>>();
builder.Services.AddScoped<IGenericRepository<BiohackingApi.Web.Models.Motivation>, GenericRepository<BiohackingApi.Web.Models.Motivation>>();
builder.Services.AddScoped<IGenericRepository<BiohackingApi.Web.Models.Biohack>, GenericRepository<BiohackingApi.Web.Models.Biohack>>();
builder.Services.AddScoped<IGenericRepository<BiohackingApi.Web.Models.Journal>, GenericRepository<BiohackingApi.Web.Models.Journal>>();
builder.Services.AddScoped<IMotivationBiohackRepository, MotivationBiohackRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Biohacking Journal API", 
        Version = "v1",
        Description = "A RESTful API for managing biohacking routines and personal motivations"
    });
    
    // Include XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biohacking Journal API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
