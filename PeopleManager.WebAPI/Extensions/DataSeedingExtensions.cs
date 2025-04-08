using PeopleManager.Application.Interfaces.Persistence;
using PeopleManager.Domain.Entities;
using PeopleManager.Infrastructure.Data;

namespace PeopleManager.WebAPI.Extensions
{
    public static class DataSeedingExtensions
    {
        public static async Task SeedDatabaseAsync(this WebApplication app) // Extension method cho WebApplication
        {
            // Tạo một scope để resolve các services (như Repository)
            // Vì seeding thường chỉ chạy một lần khi khởi động, việc tạo scope ở đây là hợp lý.
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                Console.WriteLine("Starting database seeding process...");
                try
                {
                    var personRepo = services.GetRequiredService<IRepository<Person, Guid>>();
                    bool personDataExists = (await personRepo.GetAllAsync()).Any();
                    if (!personDataExists)
                    {
                        Console.WriteLine("Person data not found. Seeding initial data...");
                        var initialPeople = DummyData.GetPeople();
                        // Gọi logic seed thực tế từ Infrastructure (ví dụ: DataSeeder)
                        // Truyền phương thức AddAsync của instance repo đã resolve vào
                        await DataSeeder.SeedAsync<Person, Guid>(initialPeople, personRepo.AddAsync);
                    }
                    else
                    {
                        Console.WriteLine("Person data already exists. Skipping seeding.");
                    }

                    // --- Seed các loại dữ liệu khác nếu có ---
                    Console.WriteLine("Database seeding process finished.");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>(); //logger nếu cần
                    logger.LogError(ex, "An error occurred during database seeding.");
                }
            }
        }
    }
}
