using Microsoft.EntityFrameworkCore;
using server_real_estate.Data;
using server_real_estate.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IRealEstatateDbContext, RealEstateDbContext>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IListService, ListService>();
builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();


builder.Services.AddDbContext<RealEstateDbContext>(options =>
{
    var connectionString = Environment.GetEnvironmentVariable("REAL_ESTATE_DB_CONNECTION_STRING");
    if (connectionString == null)
    {
        throw new ArgumentNullException("REAL_ESTATE_DB_CONNECTION_STRING");
    }
    options.UseSqlServer(connectionString);
});

builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(
        c =>
        {
            c.SwaggerEndpoint("/openapi/v1.json", "My API V1");
        }

    );
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
