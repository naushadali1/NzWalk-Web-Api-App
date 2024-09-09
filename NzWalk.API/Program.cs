using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NzWalk.API.Data;
using NzWalk.API.Mappers;
using NzWalk.API.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Serilog;
using NzWalk.API.Middlewares;

// Create the web application builder
var builder = WebApplication.CreateBuilder(args);

// Retrieve connection strings for the databases from configuration
var NzWalkConString = builder.Configuration.GetConnectionString("NzWalkConString");
var NzWalkAuthConString = builder.Configuration.GetConnectionString("NzWalkAuthConString");

// Configure logging using Serilog
var logger = new LoggerConfiguration()
    .WriteTo.Console() // Log to console
    .WriteTo.File("Logs/NzWalk_log.txt", rollingInterval: RollingInterval.Day) // Log to file with daily rolling
    .MinimumLevel.Information() // Set minimum log level to Information
    .CreateLogger();

builder.Logging.ClearProviders(); // Clear default logging providers
builder.Logging.AddSerilog(logger); // Add Serilog as the logging provider

// Add MVC controllers to the service container
builder.Services.AddControllers();

// Add HttpContextAccessor to access HTTP context
builder.Services.AddHttpContextAccessor();

// Configure Swagger/OpenAPI for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NZ Walk App", Version = "v1" }); // Set Swagger title and version
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Configure Entity Framework Core to use SQL Server for database contexts
builder.Services.AddDbContext<NZWalkDbContext>(options => options.UseSqlServer(NzWalkConString));
builder.Services.AddDbContext<NZWalkAuthDbContext>(options => options.UseSqlServer(NzWalkAuthConString));

// Register repository implementations for dependency injection
builder.Services.AddScoped<IRegionRepository, SqlRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRrpository, LocalImageRepository>();

// Configure AutoMapper for mapping objects
builder.Services.AddAutoMapper(typeof(ProfileMappers));

// Configure Identity for authentication and authorization
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalk")
    .AddEntityFrameworkStores<NZWalkAuthDbContext>()
    .AddDefaultTokenProviders();

// Configure Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false; // Passwords do not require non-alphanumeric characters
    options.Password.RequireDigit = false; 
    options.Password.RequiredLength = 8; 
    options.Password.RequireLowercase = false; 
    options.Password.RequireUppercase = false; 
    options.Password.RequiredUniqueChars = 1; 
});

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option => option.TokenValidationParameters = new TokenValidationParameters
        {
        ValidateIssuer = true, // Validate the token issuer
        ValidateAudience = true, // Validate the token audience
        ValidateLifetime = true, // Validate the token lifetime
        ValidateIssuerSigningKey = true, // Validate the token signing key
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Set valid issuer from configuration
        ValidAudience = builder.Configuration["Jwt:Audience"], // Set valid audience from configuration
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Set signing key from configuration
        });

// Build the web application
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
    {
    app.UseSwagger(); // Enable Swagger in development environment
    app.UseSwaggerUI(); // Enable Swagger UI for API documentation
    }

app.UseMiddleware<ExceptionHandlerMiddelware>(); // Use custom exception handling middleware

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization(); // Enable authorization middleware

// Serve static files from the "Images" directory
app.UseStaticFiles(new StaticFileOptions
    {
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
    });

// Map API controllers to routes
app.MapControllers();

// Run the application
app.Run();
