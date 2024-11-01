using Microsoft.EntityFrameworkCore;
using TaskService.Data;
using TaskService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var DevelopmentCorsPolicy = "DevelopmentCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: DevelopmentCorsPolicy,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200").WithMethods("POST", "PUT", "DELETE", "GET").AllowAnyHeader();
                      });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
    options.Audience = builder.Configuration["Auth0:Audience"];
});

builder.Services.AddControllers();
builder.Services.AddDbContext<ToDoTaskContext>(opt =>
    opt.UseSqlite("Data Source=database.dat"));

builder.Services.AddScoped<ToDoTaskService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
      {
          c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskService", Version = "v1.0.0" });

          string securityDefinitionName = "Bearer";

          var securitySchema = new OpenApiSecurityScheme
          {
              Description = "Using the Authorization header with the Bearer scheme.",
              Name = "Authorization",
              In = ParameterLocation.Header,
              Type = SecuritySchemeType.Http,
              Scheme = "bearer",
              Reference = new OpenApiReference
              {
                  Type = ReferenceType.SecurityScheme,
                  Id = "Bearer"
              }
          };

          OpenApiSecurityRequirement securityRequirement = new OpenApiSecurityRequirement
          {
              { securitySchema, new[] { "Bearer" } }
          };

          c.AddSecurityDefinition(securityDefinitionName, securitySchema);

          c.AddSecurityRequirement(securityRequirement);
      });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(DevelopmentCorsPolicy);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
