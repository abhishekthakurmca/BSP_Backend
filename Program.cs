using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for file logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/app-log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

/*
builder.Host.ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseStartup<Startup>();
});
*/

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<IEmailService, DummyEmailService>();

    // JWT SETTINGS
//var app = builder.Build();

    // Access the secret key from the configuration
    var secretKey = builder.Configuration["JwtSettings:SecretKey"];
    


    if (string.IsNullOrEmpty(secretKey))
    {
        Log.Error("JwtSettings:SecretKey is not set or is null.");
        secretKey = "erEYtBnhaM3NO7+esan9ThcXOUtSlXgq4yE5CwAIF5Q=";
    }
    else
    {
        Log.Information("JwtSettings:SecretKey retrieved successfully.");
    }




    var key = Encoding.ASCII.GetBytes(secretKey);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });




// This is the code to allow sources to access the api
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3001","https://localhost:3001", "http://localhost:3000","https://localhost:3000", "https://newintestserver.xyz")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21)))); // Ensure the MySQL version matches your setup


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Build Start Test",
        Version = "v1",
        Description = "Build Start Test API"
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // This shows detailed error information during development.
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDeveloperExceptionPage(); // This shows detailed error information during development.
    app.UseSwagger();
    app.UseSwaggerUI();

    //app.UseExceptionHandler("/Home/Error");
    //app.UseHsts();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/testdata", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new TestData
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetTestData")
.WithOpenApi();

app.UseRouting();

app.UseCors("AllowedOrigins");
app.UseAuthorization();
app.MapControllers();

// Enable serving static files from wwwroot
app.UseStaticFiles(); 

/*
    // Check if the wwwroot directory exists and create it if it doesn't
    var uploadsFolder = Path.Combine(app.Environment.WebRootPath ?? throw new DirectoryNotFoundException("wwwroot folder not found"), "uploads");

    if (!Directory.Exists(uploadsFolder))
    {
        Directory.CreateDirectory(uploadsFolder);
    }

    // Serves static files from uploads directory explicitly
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(app.Environment.WebRootPath, "uploads")),
        RequestPath = "/uploads"
    });
*/

app.Run();

record TestData(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
