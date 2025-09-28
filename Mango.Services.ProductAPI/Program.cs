
using Mango.Services.ProductAPI.Automapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.AddSwaggerConfiguration();

            builder.AddAppAuthentication();

            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("ProductApiConnectionString"));
            });

            builder.Services.AddAutoMapper(typeof(MapperConfig).Assembly);

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

            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                await dbContext.Database.MigrateAsync();
            }

            app.Run();

            app.Run();
        }
    }
}
