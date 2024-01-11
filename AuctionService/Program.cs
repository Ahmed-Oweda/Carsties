
using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

namespace AuctionService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            //add the db context service 
            builder.Services.AddDbContext<AuctionDbContext>(opt =>
            {
                opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            //initialize the database and seed data 
            try
            {
                DbInitiliazer.InitDb(app);
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }

            app.Run();
        }
    }
}