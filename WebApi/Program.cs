using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Helpers.Repositories;
using WebApi.Helpers.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Contexts
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Sql")));

// Repositories
builder.Services.AddScoped<GroupRepo>();
builder.Services.AddScoped<RoleRepo>();
builder.Services.AddScoped<UserGroupsRepo>();
builder.Services.AddScoped<UserRepo>();

// Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserGroupsService>();
builder.Services.AddScoped<RoleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
