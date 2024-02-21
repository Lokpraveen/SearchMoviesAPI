using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SearchMoviesAPI.Authentication;
using SearchMoviesAPI.Interface;
using SearchMoviesAPI.Model;
using SearchMoviesAPI.Services;
using SearchMoviesAPI.Exceptions;

var builder = WebApplication.CreateBuilder(args);
string ValidAudience = builder.Configuration["JWT:ValidAudience"];
string Secret = builder.Configuration["JWT:Secret"];
string ValidIssuer = builder.Configuration["JWT:ValidIssuer"];
string conn = builder.Configuration.GetConnectionString("ConnStr");
var users = builder.Configuration.GetSection("Users");
var movies = builder.Configuration.GetSection("Movies");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = ValidAudience,
                    ValidIssuer = ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret))
                };
            });

builder.Services.AddTransient<IMoviesService, MoviesService>();
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
builder.Services.Configure<List<LoginModel>>(users);
builder.Services.Configure<List<Movies>>(movies);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

