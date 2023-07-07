using MongoDB.Driver;
using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var configuration = builder.Configuration;

// Get the connection string from the appsettings.json file
builder.Services.AddSingleton<IMongoClient>(new MongoClient(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetService<IMongoClient>();
    return client.GetDatabase("easyrecovery");
});

var uploadDirectory = configuration.GetValue<string>("UploadDirectory");
if (!string.IsNullOrEmpty(uploadDirectory))
{
    uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), uploadDirectory);
    if (!Directory.Exists(uploadDirectory))
    {
        Directory.CreateDirectory(uploadDirectory);
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}");

app.Run();

