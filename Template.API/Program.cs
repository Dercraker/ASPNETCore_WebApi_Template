using Template.API.Extensions;
using Template.EFCore.IOC;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add Controllers to services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTemplateContext(builder.Configuration);

builder.Services.ConfigureSwagger();
builder.Services.ConfigureCors(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddJWT(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.AddAutoMapperConfiguration();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
    app.Services.ConfigureDatabase();

bool displaySwagger = builder.Configuration.GetValue<bool>("DisplaySwagger");
if (displaySwagger || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        options.DisplayOperationId();
        options.DisplayRequestDuration();
        options.EnableFilter();
        options.EnableTryItOutByDefault();
    });
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
    protected Program()
    {
    }
}