using TechMailGuard.Application;
using TechMailGuard.Domain.Interfaces;
using TechMailGuard.Infrastructure;
using TechMailGuard.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();


builder.Services.AddHttpClient<IEmailUnsubscribeService, EmailUnsubscribeService>();
builder.Services.AddScoped<IGmailService, GmailService>();
builder.Services.AddHttpClient<IGeminiService, GeminiService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("https://localhost:7169", "http://localhost:5119") 
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANT : Le CORS doit être activé AVANT le mapping des contrôleurs
app.UseCors("AllowBlazor");

app.UseAuthorization();

app.MapControllers();

app.Run();