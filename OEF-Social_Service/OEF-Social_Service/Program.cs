using Microsoft.Extensions.Configuration;
using OEF_Social_Service.Composition;
using OEF_Social_Service.Composition.Installer;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using EventBus.Messages.Common;
using OEF_Social_Service.EventBus.Consumer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Social_Service.Authentication;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));

string domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = domain;
        options.Audience = builder.Configuration["Auth0:Audience"];
        // If the access token does not have a `sub` claim, `User.Identity.Name` will be `null`. Map it to a different claim by setting the NameClaimType below.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("read:user", policy => policy.Requirements.Add(new HasScopeRequirement("read:user", domain)));
});
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
