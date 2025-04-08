
using PeopleManager.Application.Interfaces.Persistence;
using PeopleManager.Application.Interfaces.Services;
using PeopleManager.Application.Services;
using PeopleManager.Domain.Entities;
using PeopleManager.Infrastructure.Persistence;
using PeopleManager.WebAPI.Extensions;

namespace PeopleManager.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IRepository<Person, Guid>, Repository<Person, Guid>>();
            // PersonService depend on IRepository<Person, Guid>, DI will automatically resolve it
            builder.Services.AddScoped<IPersonService, PersonService>();

            var app = builder.Build();

            await app.SeedDatabaseAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            //app.Run();
            await app.RunAsync();
        }
    }
}
