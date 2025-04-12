using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Postech.Hackathon.Agenda.Api.Settings;
using Postech.Hackathon.Agenda.Ioc;
using Prometheus;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


builder.Services.AddSwagger();

builder.Services.Configure<AspNetCoreTraceInstrumentationOptions>(options =>
{
    // Filter out instrumentation of the Prometheus scraping endpoint.
    options.Filter = ctx => ctx.Request.Path != "/metrics";
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(b =>
    {
        b.AddService("PostechFase2");
    })
    .WithTracing(b => b
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter())
    .WithMetrics(b => b
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation()
        .AddProcessInstrumentation()
        .AddPrometheusExporter());

builder.Services.UseHttpClientMetrics();


// Configurar autenticação JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
        ClockSkew = TimeSpan.Zero
    };
});

// Registrar serviços
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Adicionar middleware de tratamento de exceções global
app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

// Adicionar middleware de autenticação
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapMetrics();

app.UseMetricServer();
app.UseHttpMetrics();
app.UseOpenTelemetryPrometheusScrapingEndpoint();

await app.RunAsync();