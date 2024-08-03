using SignalR_Project.Business;
using SignalR_Project.Hubs;
using SignalR_Project.Models;
using SignalR_Project.Subscription;
using SignalR_Project.Subscription.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddTransient<MyBusiness>();

// Uygulama boyunca dinleyici olması için singleton belirtiyoruz.
builder.Services.AddSingleton<DatabaseSubscription<Employee>>();
builder.Services.AddSingleton<DatabaseSubscription<Sale>>();

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

app.UseDatabaseSubscription<DatabaseSubscription<Sale>>("Sales");
app.UseDatabaseSubscription<DatabaseSubscription<Employee>>("Employees");

//app.MapHub<MyHub>("/myhub");
//app.MapHub<MessageHub>("/messagehub");
//app.MapHub<ChatHub>("/chathub");
app.MapHub<SalesHub>("saleshub");

app.Run();

