using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using server_real_estate.Database;
using server_real_estate.Extensions;
using server_real_estate.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.Logging;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configure = builder.Configuration;
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = configure["JwtSettings:issuer"],
    ValidAudience = configure["JwtSettings:audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configure["JwtSettings:key"]!)),
    // Allow to use seconds for expiration of token
    // Required only when token lifetime less than 5 minutes
    // THIS ONE
    ClockSkew = TimeSpan.Zero
};
builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidationParameters;
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError("Authentication failed: {Error}", context.Exception.Message);
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("Token validation failed: {Error}", context.AuthenticateFailure?.Message);
            return Task.CompletedTask;
        }
    };
})
.AddCookie(IdentityConstants.ApplicationScheme);


builder.Services.AddSingleton(tokenValidationParameters);
IdentityModelEventSource.ShowPII = true;

builder.Services.AddIdentityCore<User>()
.AddEntityFrameworkStores<RealEstateDbContext>()
.AddSignInManager<SignInManager<User>>()
.AddDefaultTokenProviders()
.AddApiEndpoints();


builder.Services.AddScoped<IRealEstatateDbContext, RealEstateDbContext>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IListService, ListService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
    
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<RealEstateDbContext>(options =>
{
    var connectionString = Environment.GetEnvironmentVariable("REAL_ESTATE_DB_CONNECTION_STRING");
    if (connectionString == null)
    {
        throw new ArgumentNullException("REAL_ESTATE_DB_CONNECTION_STRING");
    }
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(
        c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); // Ensure the endpoint matches the generated OpenAPI document     
        }
    );
    app.ApplyMigration();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/api").MapIdentityApi<User>();

app.MapControllers();

app.Run();
