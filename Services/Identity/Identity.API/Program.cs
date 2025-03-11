using Identity.Application;
using Identity.Persistence;
using Identity.Persistence.Contexts;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Custom services
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

// Accessor
builder.Services.AddHttpContextAccessor();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("default", new OpenApiInfo { Title = "API" });
});

// APP
var app = builder.Build();

// Dev
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
        var dbContext = services.GetRequiredService<IdentityDbContext>();
        dbContext.Database.EnsureCreated();
    }
}

// CORS
app.UseCors("all");

// Auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
