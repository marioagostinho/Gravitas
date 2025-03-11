using Microsoft.OpenApi.Models;
using Organization.Persistence;
using Organization.Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Custom services
builder.Services.AddPersistenceServices(builder.Configuration);

// API
builder.Services.AddControllers();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("all", builder => builder.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
});

builder.Services.AddHttpContextAccessor();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("default", new OpenApiInfo { Title = "API" });
});

// APP
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/default/swagger.json", "API");
    });

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<OrganizationDbContext>();
        dbContext.Database.EnsureCreated();
    }
}

// CORS
app.UseCors("all");

app.UseAuthorization();
app.MapControllers();

app.Run();
