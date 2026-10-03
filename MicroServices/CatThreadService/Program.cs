using System;
using MicroServices.CatThreadService.Data;
using Microsoft.EntityFrameworkCore;

using CatThreadService.Data;

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
builder.Services.AddScoped<IThreadRepo, ThreadRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>(); // Đăng ký CategoryRepo
builder.WebHost.UseUrls("http://*:5001");

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
app.UseEndpoints( endpoints=>
{
    endpoints.MapControllers();
    // Định nghĩa các endpoint ở đây
}); ;

app.Run();
