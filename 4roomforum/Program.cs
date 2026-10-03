using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using _4roomforum.Services.Implements;
using _4roomforum.Services.Interfaces;
using _4roomforum.Sockett;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICategoryService, CategoryServiceImpl>();
builder.Services.AddScoped<IThreadService, ThreadServiceImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();
builder.Services.AddScoped<IPostService, PostServiceImpl>();
builder.Services.AddScoped<IReplyService, ReplyServiceImpl>();
builder.Services.AddHttpClient<CategoryServiceImpl>();
builder.Services.AddHttpClient<UserServiceImpl>();
builder.Services.AddHttpClient<ThreadServiceImpl>();
builder.Services.AddHttpClient<PostServiceImpl>();
builder.Services.AddHttpClient<ReplyServiceImpl>();
builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Đảm bảo có DefaultSignInScheme

    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddGoogle(options =>
{
    var googleAuth = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuth["ClientId"];
    options.ClientSecret = googleAuth["ClientSecret"];
    options.CallbackPath = new PathString("/Login/SignInGoogle");
})
.AddCookie(options =>
{
    options.LoginPath = "/Login/SignIn";
    options.LogoutPath = "/Login/SignOut";
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = 401; // API trả về Unauthorized
            }
            else
            {
                context.Response.Redirect("/Login/SignIn");
            }
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // Đảm bảo thêm Authentication middleware
app.UseAuthorization();
app.MapHub<CommentSocket>("/comment");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
