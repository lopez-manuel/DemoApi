using DemoApi.Data;
using DemoApi.Repository;
using DemoApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DemoApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContext<DemoApiDbContext>(opt =>
            opt.UseSqlServer(
                builder.Configuration.GetConnectionString("DemoApiDB")
                ));

        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddAutoMapper(cf =>{},typeof(Program).Assembly);
        
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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