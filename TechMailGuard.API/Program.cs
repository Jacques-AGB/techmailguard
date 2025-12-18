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

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();

app.Run();