using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoBoards.Storage.Extensions;
using ToDoBoards.Api.Extensions;
using ToDoBoards.Api.Features.RequestRateLimit.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersions();
builder.Services.AddSwagger();
builder.Services.AddDbStorage(builder.Configuration);
builder.Services.AddApiServices();
builder.Services.AddRequestRateLimit(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ApplySwagger();
}

app.UseHttpsRedirection();
app.UseRequestRateLimit(app.Configuration);
app.UseAuthorization();
app.MapControllers();

app.Run();