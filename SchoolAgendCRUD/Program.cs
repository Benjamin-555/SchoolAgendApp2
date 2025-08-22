using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SchoolAgend.Application.Contracts;
using SchoolAgend.Infrastructure.Data.Repositories;
using SchoolAgendCRUD.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<SchoolAgendCRUDDbContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("MainConnection")));
// Add services to the container.

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<ITaskIRepository, TaskIRepository>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
