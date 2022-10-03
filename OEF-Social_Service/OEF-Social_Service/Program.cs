using Microsoft.Extensions.Configuration;
using OEF_Social_Service.Composition;
using OEF_Social_Service.Composition.Installer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
// Add services to the container.
new DbInstaller().InstallServices(builder.Services, builder.Configuration);
new ServiceInstaller().InstallServices(builder.Services, builder.Configuration);
new LogicInstaller().InstallServices(builder.Services, builder.Configuration);
builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
