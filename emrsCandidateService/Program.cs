using CRUDENTITY.DataContext;
using CRUDENTITY.UOWRepository;
using CRUDENTITY.UOWRepository.EntityRepository;
using CRUDENTITY.UOWRepository.GenericeRepository;
using emrsCandidateService.Middleware;
using emrsCandidateService.SignalHub;
using Microsoft.EntityFrameworkCore;
using SERVICEAPP.ServiceLayer;

var builder = WebApplication.CreateBuilder(args);

//var connectionString = Environment.GetEnvironmentVariable("DbDockerConnection") ??
//                        builder.Configuration.GetConnectionString("DefaultConnection");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Repository Layer

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IRolePagePermissionRepository, RolePagePermissionRepository>();
//builder.Services.AddScoped<IRoleWisePagePermissionRepository, RoleWisePagePermissionRepository>();
builder.Services.AddScoped<IStudentAppointmentRepository, StudentAppointmentRepository>();
builder.Services.AddScoped<IAppointmentLetterRepository, AppointmentLetterRepository>();
builder.Services.AddScoped<IVisitorRepository, VisitorRepository>();

#endregion

//Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

#region Service Layer
builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<ICaptchaService, CaptchaService>();
//builder.Services.AddScoped<IViewRenderService, ViewRenderService>();
builder.Services.AddScoped<IStudentAppointmentService, StudentAppointmentService>();
builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddScoped<IVisitorService, VisitorService>();
#endregion

#region Cors

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngular",
//        policy =>
//        {
//            policy.WithOrigins("http://localhost:4201") // your Angular URL
//                  .AllowAnyHeader()
//                  .AllowAnyMethod()
//                  .AllowCredentials(); // REQUIRED for SignalR;
//        });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

#endregion
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
//app.UseCors("AllowAngular");
app.UseCors("AllowAll");


// ADD YOUR MIDDLEWARE HERE (BEST POSITION)
//app.UseMiddleware<VisitorTrackingMiddleware>();

app.UseAuthorization();

app.MapControllers();

// THEN MAP SIGNALR HUB
app.MapHub<VisitorHub>("/visitorHub");

app.Run();
