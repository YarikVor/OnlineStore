using AutoMapper;
using OnlineStore.Mappers;

namespace OnlineStore;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers();

        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGenWithScheme();

        services
            .AddCors();

        services
            //.AddEntityFrameworkNpgsql()
            .AddApplicationIdentity()
            .AddConfigurationIdentity()
            .AddContexts(_configuration);

        services
            .AddOnlineStoreAutoMapper();

        services
            .AddJwtAuthentication()
            .AddTokenGenerator();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();


        
        app.UseCors();


        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.UseCertificateForwarding();
    }
}