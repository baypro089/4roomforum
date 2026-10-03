using System;
using MicroServices.UserServices.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using UserServices.Data;
var builder = WebApplication.CreateBuilder(args);

// Cấu hình DbContext với MySQL
builder.Services.AddDbContext<AppDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Thêm các service cần thiết
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program)); // Sử dụng typeof(Program) thay cho typeof(Startup)

builder.Services.AddScoped<IUserRepo, UserRepo>(); // Đăng ký CategoryRepo
builder.Services.AddScoped<IRoleRepo, RoleRepo>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    dbContext.Database.Migrate();
}

// Cấu hình middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();

// Cấu hình API endpoints
app.MapControllers();

app.Run();
