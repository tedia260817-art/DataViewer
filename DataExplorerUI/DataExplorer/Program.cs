
using DataExplorer.Services;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddScoped<AlbumGraphQLService>();
builder.Services.AddScoped<PostGraphQLService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();
app.MapControllers();

app.Run();
