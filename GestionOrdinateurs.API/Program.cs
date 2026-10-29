using GestionOrdinateurs.API.Services;
using GestionOrdinateurs.API.Data;
using Microsoft.EntityFrameworkCore;
using GestionOrdinateurs.Shared;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5156") //Url du clien Blazor
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials() // si cookies
              .WithExposedHeaders("Authorization");
    });
});


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GestionOrdinateurs",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrez : Bearer {token}"
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// Configuration JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "";
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
    options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                Console.WriteLine($"Utilisateur utilisant un Token Valide --- {context.Principal?.Identity?.Name ?? "Anonymous"}");
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Token Invalide");
                return Task.CompletedTask;
            }
        };
});


builder.Services.AddScoped<IOrdinateurService,OrdinateursService>();
builder.Services.AddScoped<ReparationService>();
builder.Services.AddDbContext<ParcInformatiqueContext>();

var app = builder.Build();
using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ParcInformatiqueContext>();
    
    db.Database.Migrate();

    if(!db.Utilisateurs.Any())
    {
        db.Utilisateurs.AddRange(
            new User { Login="user1", Role=RoleEnum.Admin },
            new User { Login="user2", Role=RoleEnum.Gestionnaire },
            new User { Login="user3", Role=RoleEnum.Gestionnaire }
        );
        db.SaveChanges();
    }
}

// Middleware
app.UseCors("AllowBlazorClient");
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();
