using hacker_news.Api.Services;
using hacker_news.Api.Repositories;

namespace hacker_news.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Register services
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddHttpClient();
            services.AddMemoryCache();

            services.AddScoped<IStoryService, StoryService>();
            services.AddScoped<IStoryApiClient, StoryApiClient>();
            services.AddScoped<IStoryRepository, StoryRepository>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowWebsite", policy =>
                {
                    policy.WithOrigins(
                        "https://www.jacobmoisan.com",  // With www
                        "https://jacobmoisan.com"        // Without www
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }

        // Configure middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowWebsite");

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}