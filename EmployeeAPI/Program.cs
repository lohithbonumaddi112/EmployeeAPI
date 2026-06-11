using EmployeeAPI.Filters;
using EmployeeAPI.Interfaces;
using EmployeeAPI.Interfaces.IRepositories;
using EmployeeAPI.Interfaces.IServices;
using EmployeeAPI.Models;
using EmployeeAPI.Repository;
using EmployeeAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDbContext<DbemployeeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DF1")));
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddScoped<ICreateEmployee, CreateEmployee>();
builder.Services.AddScoped<IGetEmployee,GetEmployeeService>();
builder.Services.AddScoped<IEditEmployee, EditEmployeeService>();
builder.Services.AddScoped<IDeleteEmployee, DeleteEmployeeService>();
builder.Services.AddScoped<IRegisterUser, RegisterUserService>();
builder.Services.AddScoped<IGetEmployeeRepos, GetEmployeeRepos>();
builder.Services.AddScoped<CustomAuth>();
builder.Services.AddScoped<ActionFilter>();
builder.Services.AddScoped<CustExceptionFilter>();
builder.Services.AddScoped<ResourceFilter>();
builder.Services.AddScoped<ResultFilter>();
builder.Services.AddResponseCaching();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
      options.TokenValidationParameters =
        new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!))
        }


    );

builder.Services.AddSwaggerGen(options =>
{

   
    options.SwaggerDoc("v1",
       new OpenApiInfo
       {
           Title = "EmployeeAPI",
           Version = "v1"
       });

    options.SwaggerDoc("v2",
        new OpenApiInfo
        {
            Title = "EmployeeAPI",
            Version = "v2"
        });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddApiVersioning(options => 

{ options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(2, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;


}


);
builder.Services.AddAuthorization(options =>

{
    options.AddPolicy("RootUsers", policy => policy.RequireRole("Root"));
});

var app = builder.Build();


    app.UseSwagger();
   app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "EmployeeAPI V1");

        options.SwaggerEndpoint(
            "/swagger/v2/swagger.json",
            "EmployeeAPI V2");
    });




// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseMiddleware<EmployeeAPI.Middleware.ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();
app.MapControllers();

app.Run();
