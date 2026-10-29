using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.SessionStorage;
using GestionOrdinateurs.Client;
using GestionOrdinateurs.Client.Pages;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());

builder.Services.AddScoped<JwtHandler>();

//Authentification
builder.Services.AddHttpClient<IAuthentificationService, AuthentificationService>("HttpClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5209");// Url Authentifcation 
   
})
.AddHttpMessageHandler<JwtHandler>();

//Lien API pour les ordinateurs -> Client
builder.Services.AddHttpClient<IComputerService, ComputerService>("HttpClientComputer", client =>
{
    client.BaseAddress = new Uri("http://localhost:5010"); //Url Gateway
   
});

//Lien API pour les reparation -> Client
builder.Services.AddHttpClient<IReparationService, ReparationService>("HttpClientReparation", client =>
{
    client.BaseAddress = new Uri("http://localhost:5010"); //Url Gateway
   
});

await builder.Build().RunAsync();
