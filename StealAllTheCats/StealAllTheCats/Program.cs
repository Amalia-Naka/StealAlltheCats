
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using StealAllTheCats.Application.Commands;
using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.Application.Services;
using StealAllTheCats.DAL.Data;
using StealAllTheCats.DAL.Helpers;
using System.Windows.Input;

namespace StealAllTheCats
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
            builder.Services.AddControllers();

            // Register configuration in DI
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // take CatService and give http request
            builder.Services.AddHttpClient();
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("StealAllTheCats.DAL") //   migrations assembly
                );
            });

            ////register context of db
            // builder.Services.AddDbContext<DataContext>(options =>
            // {
            //   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            // });
            builder.Services
                .AddScoped<IDbExtension, DbExtension>();

            builder.Services
                            .Scan(scan =>
                            scan.FromAssemblyOf<FetchCatsCommand>()
                            .AddClasses(classes => classes
                            .AssignableTo<Application.Interfaces.ICommand>())
                            .AsSelf()
                            .WithScopedLifetime());

            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped<ICatService, CatService>();



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
        }
    }
}
