using DataExplorerUI.Components;
using DataExplorerModels;
using DataExplorerUI.Services; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<AlbumService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://graphqlzero.almansi.me/api") });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseWebSockets(); 
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
