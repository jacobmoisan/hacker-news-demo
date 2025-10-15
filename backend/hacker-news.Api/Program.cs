using hacker_news.Api.Services;
using hacker_news.Api.Repositories;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddHttpClient();
        builder.Services.AddMemoryCache();

        builder.Services.AddScoped<IStoryService, StoryService>();
        builder.Services.AddScoped<IStoryApiClient, StoryApiClient>();
        builder.Services.AddScoped<IStoryRepository, StoryRepository>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}
