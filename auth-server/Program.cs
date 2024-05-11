using auth_server.Identity;
using auth_server.service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddDbContext<AuthContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
  
    options.Password.RequireUppercase = true;
})
     .AddEntityFrameworkStores<AuthContext>()
     .AddDefaultTokenProviders();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var useIdentity = builder.Configuration["UseIdentity"];
switch (useIdentity)
{
    case "identity":
        builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
        break;
    case "ms-acceess":
        // builder.Services.AddScoped(typeof(IAuthService), typeof(MsAccessAuthService));
        break;
    case "other":
        builder.Services.AddScoped(typeof(IAuthService), typeof(OtherAuthService));
        break;
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(o =>
{
    o.AllowAnyHeader();
    o.AllowAnyMethod();
    o.AllowAnyOrigin();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
