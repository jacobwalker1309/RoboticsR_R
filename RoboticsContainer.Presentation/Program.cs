using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

//Environment.SetEnvironmentVariable("ASPNETCORE_URLS", "http://localhost:4000;https://localhost:4001");

builder.WebHost.ConfigureKestrel(options =>
{
    // Set up Kestrel server options if needed
    // You can configure the endpoints here if you need custom ports or settings
    options.ListenLocalhost(4000); // HTTP port
    options.ListenLocalhost(4001, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS port
    });
});

// Add services to the container.
// these need seperated out into its own place
// Use the extension method to register services
builder.Services.AddCustomServices(builder.Configuration);

// Add controllers with views (including anti-forgery if needed)
// might have to change this to just controllers or controllers with MVC either way
builder.Services.AddControllersWithViews(); // Includes anti-forgery

// Optional: Add anti-forgery services explicitly
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN"; // Custom header name if needed
});

// Add endpoints API explorer and Swagger (if needed)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin"); // Apply CORS middleware

app.UseAuthentication(); // Ensure this is before UseAuthorization

app.UseAuthorization();

app.MapControllers();

app.Run();
