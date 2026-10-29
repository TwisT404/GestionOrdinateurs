
    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;

    var builder = WebApplication.CreateBuilder(args);

    //Autorisation d'accès à l'API (CORS)
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowBlazorClient", policy =>
        {
            policy.WithOrigins("http://localhost:5000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials() // si tu utilises cookies
                .WithExposedHeaders("Authorization");
        });
    });

    // Add YARP reverse proxy
    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));




    builder.Services.AddAuthorization();


    var app = builder.Build();

    //Activer CORS
    app.UseCors("AllowBlazorClient");

    app.MapReverseProxy();

    app.MapGet("/", () => "Gateway is running");

    app.Run();