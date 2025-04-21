using bingo_api.src.Configurations;
using bingo_api.src.Context;
using bingo_api.src.Extensions;
using bingo_api.src.IoC;
using Hangfire;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using System.Text.Json;
using bingo_api.src.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors ( options =>
{
    options.AddPolicy ( "AllowAll" , policy =>
    {
        policy.WithOrigins ( "http://homologation-bingo-dashboard.captain.localhost" , "http://localhost" , "http://localhost:4200" , "http://localhost:4300" , "https://localhost:4200" , "https://localhost:4300" )
              .AllowAnyMethod ( )
              .AllowAnyHeader ( )
              .AllowCredentials ( );
    } );
} );
builder.Services.AddApiProblemDetails ( );
builder.Services.AddControllers ( )
.AddJsonOptions ( options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add ( new System.Text.Json.Serialization.JsonStringEnumConverter ( ) );
    options.JsonSerializerOptions.Converters.Add ( new JsonTimeOnlyConverter() );
} );
builder.Services.AddEndpointsApiExplorer ( );
builder.Services.AddLogging ( );
builder.Services.AddRouting ( options => options.LowercaseUrls = true );
builder.Services.AddVersioning ( );
builder.Services.AddSwagger ( );
builder.Services.AddSwaggerGen ( );
builder.Services.AddAuthentication ( builder.Configuration );
builder.Services.AddAuthorizationPolicies ( );
builder.Services.AddAuthorization ( );
builder.Services.RegisterServices ( builder.Configuration );
builder.Services.AddHangfireServer ( );
var app = builder.Build();
app.MigrateDatabase<DataContext> ( );
app.MigrateDatabase<IdentityDataContext> ( );
app.UseCors ( "AllowAll" );
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment ( ))
{
    app.UseSwagger ( );
    app.UseSwaggerUI ( c =>
    {
        c.SwaggerEndpoint ( "/swagger/v1/swagger.json" , "My API V1" );
    } );
}
using (var scope = app.Services.CreateScope ( ))
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await DataInitializer.Seed ( context , userManager , roleManager );
}
app.UseProblemDetails ( );
app.UseWebSockets ( );
app.UseHttpsRedirection ( );
app.UseRouting ( );
app.UseAuthentication ( );
app.UseAuthorization ( );
app.UseHangfireJobs();
app.UseHangfireDashboard ( "/hangfire" , new DashboardOptions
{
    Authorization = HangFireDashboardAuthorization.AuthenticationFilters ( )
} );
app.MapControllers ( );

await app.RunAsync ( );
