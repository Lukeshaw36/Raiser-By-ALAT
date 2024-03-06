
using GROUP2.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using GROUP2.Services.GroupInvestment;
using GROUP2.Services.User;
using GROUP2.Services.UserDetails;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository, UserRepsitory>();
builder.Services.AddScoped<IValidateUserOTPRepository, ValidateUserOtpRepository>();
builder.Services.AddScoped<IUserLoginRepository, LoginRepository>();
builder.Services.AddScoped<IUserResetRepository, UserResetRepository>();



//Add automapper
builder.Services.AddAutoMapper(typeof(Program));

//Add Memory cache
builder.Services.AddMemoryCache();

builder.Services.AddScoped<IUserAccount, UserAccountRepository>();

builder.Services.AddTransient<IInterestRepository, InterestRepository>();

builder.Services.AddScoped<IInvetmentRepo, InvestmentRepository>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization Header Using The Bearer Scheme(\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddCors(options =>
{
    //options.AddPolicy("AllowAllOrigins",
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()  // Allow requests from any origin
                   .AllowAnyHeader()  // Allow any headers in the request
                   .SetIsOriginAllowed(origin => true)
                   .AllowAnyMethod() // Allow any HTTP method (GET, POST, etc.)
                   //.AllowCredentials()
                   ;
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("AllowAllOrigins");


app.UseAuthorization();

app.MapControllers();

app.Run();
