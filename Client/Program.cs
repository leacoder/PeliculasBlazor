using BlazorApp1.Client;
using BlazorApp1.Client.Repository;
using KristofferStrube.Blazor.FileSystemAccess;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton(sp => new SingletonService());
builder.Services.AddSingleton<IRepository, Repository>();
builder.Services.AddFileSystemAccessService();
builder.Services.AddTelerikBlazor();
//builder.Services.AddScoped<IJSRuntime>();

await builder.Build().RunAsync();
