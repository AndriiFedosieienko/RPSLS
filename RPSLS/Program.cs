using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using RPSLS.Applications.Contracts;
using RPSLS.Applications.Contracts.Repositories;
using RPSLS.Applications.Services;
using RPSLS.Applications.Services.Repositories;
using RPSLS.Persistence.Contexts;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string issuer = builder.Configuration["Security:Authentication:Tokens:Issuer"];
string audience = builder.Configuration["Security:Authentication:Tokens:Audience"];
string key = builder.Configuration["Security:Authentication:Tokens:Key"];

builder.Services.AddDbContext<RPSLSDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration["ConnectionStrings:RPSLSContext"]);
}, ServiceLifetime.Singleton);

builder.Services.AddAuthentication(delegate (AuthenticationOptions options)
{
	options.DefaultAuthenticateScheme = "Bearer";
	options.DefaultChallengeScheme = "Bearer";
	options.DefaultSignInScheme = "Bearer";
}).AddJwtBearer(delegate (JwtBearerOptions options)
{
	options.RequireHttpsMetadata = false;
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidIssuer = issuer,
		ValidAudience = audience,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero,
		NameClaimType = "unm"
	};
});

builder.Services.AddHttpClient();

builder.Services.AddSingleton<IChoicesRepository, ChoicesRepository>();
builder.Services.AddSingleton<IRulesRepository, RulesRepository>();
builder.Services.TryAddScoped<IChoicesService, ChoicesService>();
builder.Services.TryAddScoped<IRulesService, RulesService>();
builder.Services.TryAddScoped<IRandomService, RandomService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();