
using DemoPilotoV1.BDD;
using Microsoft.EntityFrameworkCore;

namespace DemoPilotoV1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.                  

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<BaseDeDatos>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaConexion")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //Cors
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:3000")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });

            app.UseAuthorization();



            app.UseStaticFiles();


            app.MapControllers();

            app.Run();
        }
    }
}