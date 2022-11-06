using Api;
using Api.Middlewares;
using Application;
using Infrastructure;
using Infrastructure.Data;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddContexts(builder.Configuration);
builder.Services.AddServicesAndRepositories();
builder.Services.AddMediatR(typeof(ApplicationMediatREntrypoint).Assembly);
builder.Services.AddApplicationLayer();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
builder.AddCors();
builder.AddAuthenticationWithJWT();

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
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();
