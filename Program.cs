using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DemoPilotoV1.BDD;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DemoPilotoV1.Repositorios;  // Nueva línea para importar el repositorio de pedidos
using System.Text.Json.Serialization;
using DemoPilotoV1.Clases;
using DemoPilotoV1.DTOS;
using static System.Runtime.InteropServices.JavaScript.JSType;




namespace DemoPilotoV1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;                    
                   // options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            

            // Add DbContext with MySQL
            builder.Services.AddDbContext<BaseDeDatos>(options =>
                options.UseMySQL(builder.Configuration.GetConnectionString("CadenaConexion")));

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000", "https://396d-167-60-192-17.ngrok-free.app", "https://monragittwo.github.io")
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    });
            });

            // Add authentication (optional, adjust according to your needs)
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://your-identity-server";
                    options.Audience = "api1";
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api1");
                });
            });

            // Register the new repository for pedidos
            builder.Services.AddScoped<RepoPedidos>();  // Nueva línea para agregar el repositorio de pedidos

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

           

        app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Apply CORS middleware
            app.UseCors("AllowSpecificOrigins");

            // Use authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
