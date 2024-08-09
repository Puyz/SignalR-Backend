

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using SignalR_Project.Context;
using SignalR_Project.Hubs;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AppDbContext>();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder.AllowAnyHeader()
           .AllowAnyMethod()
           .SetIsOriginAllowed((host) => true)
           .AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat-hub");

app.Map("/avatars", file =>
{
    var path = Path.Combine(Directory.GetCurrentDirectory(), "file-storage/avatars");
    if (!Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }

    file.UseFileServer(new FileServerOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "file-storage/avatars")),
        EnableDirectoryBrowsing = false
    });
});

app.Run();

