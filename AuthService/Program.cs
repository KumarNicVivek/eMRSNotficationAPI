using CRUDENTITY.DataContext;
using CRUDENTITY.UOWRepository;
using CRUDENTITY.UOWRepository.EntityRepository;
using CRUDENTITY.UOWRepository.GenericeRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SERVICEAPP.ServiceLayer;

var builder = WebApplication.CreateBuilder(args);

var connectString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectString);
});

// Add services to the container.

builder.Services.AddControllers();

//JWT Authentication

builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    })
    .AddJwtBearer(options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("JWT key is not configured.");
        }

        var keyBytes = Convert.FromBase64String(jwtKey);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer <your JWT token>"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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

// Register Memory Cache (Required for CAPTCHA)
builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

#region Repository Layer

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRolePagePermissionRepository, RolePagePermissionRepository>();
builder.Services.AddScoped<IRoleWisePagePermissionRepository, RoleWisePagePermissionRepository>();
builder.Services.AddScoped<IStudentAppointmentRepository, StudentAppointmentRepository>();
builder.Services.AddScoped<IAppointmentLetterRepository, AppointmentLetterRepository>();

#endregion

//Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

#region Service Layer
builder.Services.AddScoped<IAuthService, AuthServices>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICaptchaService, CaptchaService>();
//builder.Services.AddScoped<IViewRenderService, ViewRenderService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
// Enable serving static files from wwwroot
app.UseStaticFiles();
app.UseCors("DevCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
