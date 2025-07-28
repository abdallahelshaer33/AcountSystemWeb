

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AccountingSystemAPI.Data;


var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    WebRootPath = "wwwroot"
});

// connection string
var connectionString  = builder.Configuration.GetConnectionString("DefaultConnection") ?? Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");
var jwtsetting = builder.Configuration.GetSection("JWT");
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDbContext<TafteshDbContext>(op => op.UseSqlServer(sql));
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddSession(s =>
{
    s.IdleTimeout = TimeSpan.FromHours(1);
    s.Cookie.HttpOnly = true;
    s.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();
//builder.Services.AddDistributedMemoryCache();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapFallbackToFile("index.html");
app.Run();
