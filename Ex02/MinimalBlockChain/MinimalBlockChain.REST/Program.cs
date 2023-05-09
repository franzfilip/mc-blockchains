using MinimalBlockChain.BlockChain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MinimalBlockChain.REST
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
            builder.Services.AddSingleton<BlockChain.BlockChain>();
            //builder.Services.Addnew

            //var serializerSettings = new JsonSerializerSettings
            //{
            //    ContractResolver = new DefaultContractResolver
            //    {
            //        NamingStrategy = new 
            //    }
            //};


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}