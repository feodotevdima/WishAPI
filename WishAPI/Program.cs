using Application;
using Application.interfases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Presistence;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WishContext>();

builder.Services.AddHttpClient();

builder.Services.AddTransient<IWishRepository, WishRepository>();
builder.Services.AddTransient<IWishService, WishService>();

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
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = "AuthenticationService",
            ValidAudience = "APIUsers",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YkR7g4$&9hDfT8*5mWjXq3E@aBzP4VqC"))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
