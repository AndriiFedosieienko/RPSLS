using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using RPSLS.Applications.Contracts;
using RPSLS.Applications.Contracts.Repositories;
using RPSLS.Applications.Services;
using RPSLS.Applications.Services.Repositories;
using RPSLS.Domain.Entities;
using RPSLS.Persistence.Contexts;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Represents swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AuthDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration["ConnectionStrings:AuthContext"]);
}, ServiceLifetime.Singleton);
builder.Services.AddIdentity<EnhancedUser, EnhancedRole>()
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = "Bearer";
	options.DefaultChallengeScheme = "Bearer";
	options.DefaultSignInScheme = "Bearer";
})
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidIssuer = builder.Configuration["Security:Authentication:Tokens:Issuer"],
			ValidAudience = builder.Configuration["Security:Authentication:Tokens:Audience"],
			IssuerSigningKey =
							new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
								builder.Configuration["Security:Authentication:Tokens:Key"])),
			ClockSkew = TimeSpan.Zero,
			NameClaimType = "unm"
		};
		options.IncludeErrorDetails = true;
	});

builder.Services.AddAuthorization();

builder.Services.TryAddScoped<IUsersRepository, UsersRepository>();
builder.Services.TryAddScoped<IAuthService, AuthService>();
builder.Services.TryAddScoped<IEncryptionService, EncryptionService>();
builder.Services.TryAddScoped<ITokenManagementService, TokenManagementService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();