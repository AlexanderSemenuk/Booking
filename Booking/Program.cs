
using Booking.Data;
using Booking.Security;
using Booking.Services;
using Microsoft.EntityFrameworkCore;
namespace Booking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services
                    .AddEndpointsApiExplorer()
                    .AddSwaggerGen()
                    .AddScoped<PasswordHasher>()
                    .AddScoped<IUserService, UserService>()
                    .AddScoped<IBookingService, BookingService>();


            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
            });


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
