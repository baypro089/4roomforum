using PostService.Data;
using Microsoft.EntityFrameworkCore;
using PostService.Models;
using PostService.DTOs;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddScoped<IBaseRepository<Post, PostDTO, CreatePostDTO, UpdatePostDTO>,
    BaseRepository<Post, PostDTO, CreatePostDTO, UpdatePostDTO>>();

builder.Services.AddScoped<ILikeOfPostRepo, LikeOfPostRepo>();

builder.Services.AddScoped<IPostRepo, PostRepo>();
builder.Services.AddScoped<IReplyRepo, ReplyRepo>();

builder.Services.AddScoped<ILikeOfReplyRepo, LikeOfReplyRepo>();

builder.Services.AddScoped<IBaseRepository<LikeOfPost, LikeOfPostDTO, CreateLikeOfPostDTO, UpdateLikeOfPostDTO>,
    BaseRepository<LikeOfPost, LikeOfPostDTO, CreateLikeOfPostDTO, UpdateLikeOfPostDTO>>();

builder.Services.AddScoped<IBaseRepository<LikeOfReply, LikeOfReplyDTO, CreateLikeOfReplyDTO, UpdateLikeOfReplyDTO>,
    BaseRepository<LikeOfReply, LikeOfReplyDTO, CreateLikeOfReplyDTO, UpdateLikeOfReplyDTO>>();

builder.Services.AddScoped<IBaseRepository<Reply, ReplyDTO, CreateReplyDTO, UpdateReplyDTO>,
    BaseRepository<Reply, ReplyDTO, CreateReplyDTO, UpdateReplyDTO>>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers(); // Register controllers

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting(); // Enable routing

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Map controller routes
});

app.Run();