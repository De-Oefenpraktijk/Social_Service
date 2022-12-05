using Microsoft.Extensions.Configuration;
using OEF_Social_Service.Composition;
using OEF_Social_Service.Composition.Installer;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using EventBus.Messages.Common;
using OEF_Social_Service.EventBus.Consumer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<ProfileUpdatedConsumer>();

    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ReceiveEndpoint(EventBusConstants.PROFILEUPDATEDQUEUE, c =>
        {
            c.ConfigureConsumer<ProfileUpdatedConsumer>(ctx);
        });
    });
});
builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.
new DbInstaller().InstallServices(builder.Services, builder.Configuration);
new ServiceInstaller().InstallServices(builder.Services, builder.Configuration);
new LogicInstaller().InstallServices(builder.Services, builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = ApiVersion.Default; //1.0
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add cors policy
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
