var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();          // <- obligatoire
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // <- Swagger
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5000") // URL du client Blazor
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddHttpClient<IUserService, UserService>("HttpAuthAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5035"); //Url GestionOrdinateurs.API
});

var app = builder.Build();
//Activer CORS
app.UseCors("AllowBlazorClient");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();                      
app.Run();