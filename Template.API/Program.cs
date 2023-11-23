using Microsoft.CodeAnalysis;
using Template.API.Extensions;
using Template.Domain.Helper;
using Template.EFCore.IOC;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add Controllers to services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTemplateContext(builder.Configuration);

builder.Services.ConfigureSwagger();
builder.Services.ConfigureCors(builder.Configuration);
builder.Services.ConfigureOutputCache();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddGraphQLService();
builder.Services.AddJWT(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.AddAutoMapperConfiguration();
TextLogger logger = builder.Services.SetupLogger();



WebApplication app = builder.Build();
app.ConfigureExceptionHandler(logger);
await app.Services.ConfigureDatabase();

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

app.UseOutputCache();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");

app.Run();

public partial class Program
{
    protected Program()
    {
    }
}