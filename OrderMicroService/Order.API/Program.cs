//using Order.Infrastructure;
using Oder.Infrastructure;
using Order.API.CustomeMiddleware;
using Order.Core.AutoMapper;

using Order.Core.HttpClients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(OrderAutoMapper).Assembly);
builder.Services.AddOrderInfrastucture(builder.Configuration);

builder.Services.AddHttpClient<UsersMicroserviceClient>(client => {
    //client.BaseAddress = new Uri(builder.Configuration["UserMicroServiceURL"]!);
    client.BaseAddress = new Uri($"http://{builder.Configuration["UsersMicroserviceName"]}:{builder.Configuration["UsersMicroservicePort"]}");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandlingMiddleware();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
