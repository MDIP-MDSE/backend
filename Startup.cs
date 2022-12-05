using System.Text;
using System.Text.Json.Serialization;
using MDIP_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace MDIP_Backend;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        
        services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );
        
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "MDIP-MMO-Indexer", Version = "v1" });
        });

        if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "production")
            services.AddDbContext<MMOContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("prod")));
        else
            services.AddDbContext<MMOContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("prod")));
        
        services.BuildServiceProvider().GetService<MMOContext>().Database.Migrate();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    
        app.UseCors(x =>
        {
            x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://mdip.vercel.app");
        });
        
        app.UseDeveloperExceptionPage();
        
        app.UseSwagger(options =>
        {
            options.SerializeAsV2 = true;
        });
            
        app.UseSwaggerUI();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
    }
}