
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService.Data;
using SearchService.Models;
using SearchService.Services;
using System.Net;

namespace SearchService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHttpClient<AuctionServiceHttpClient>().AddPolicyHandler(GetPolicy());
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Lifetime.ApplicationStarted.Register(async () =>
            {
                try
                {
                    DBInitializer.InitilizeMongoDb(app);

                }
                catch (Exception e)
                {

                    Console.Write(e);
                }
            });
            

            app.Run();

            static IAsyncPolicy<HttpResponseMessage> GetPolicy()
                => HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                    .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
        }

    }
}