using Identity;
using Identity.Infrastructure;
using Identity.Services;
using Identity.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDB(builder.Configuration);
builder.Services.AddIdentity();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.AddCors();
builder.AddAuthenticationWithJWT();

var app = builder.Build();
app.Services.EnsureDbsAreCreated();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
