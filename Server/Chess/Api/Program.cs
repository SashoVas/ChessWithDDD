using Application;
using Infrastructure.Data;
using Infrastructure.Services;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddContexts(builder.Configuration);
builder.Services.AddServicesAndRepositories();
builder.Services.AddMediatR(typeof(ApplicationMediatREntrypoint).Assembly);
builder.Services.AddApplicationLayer();

var app = builder.Build();
using (var serviceScope = app.Services.CreateScope())
{
    serviceScope.EnsureDbsAreCreated();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
