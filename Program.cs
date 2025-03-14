using LoanManagementSystem.Data;
using LoanManagementSystem.Repositories;
using LoanManagementSystem.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configure Database Connection
builder.Services.AddDbContext<LoanDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IKycRepository, KycRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<ICreditCardRepository, CreditCardRepository>(); // ✅ Register CreditCardRepository
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<OtpService>();

// ✅ Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("http://localhost:5173") // ✅ Allow frontend
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// ✅ Add Controllers
builder.Services.AddControllers();

// ✅ Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Loan Management System API",
        Version = "v1",
        Description = "API documentation for Loan Management System",
        Contact = new OpenApiContact
        {
            Name = "Support",
            Email = "support@example.com",
            Url = new Uri("https://example.com")
        }
    });
});

var app = builder.Build();

// ✅ Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Loan Management System API v1");
        c.RoutePrefix = "swagger"; // ✅ Access at http://localhost:5272/swagger
    });
}

// ✅ Middleware
// app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // ✅ Enable CORS
app.UseAuthorization();
app.MapControllers();
app.Run();
