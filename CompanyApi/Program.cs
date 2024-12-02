using System;
using System.Reflection;
using System.Text.Json.Serialization;
using CompanyApi.Middlewares;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service;
using Repository;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
                 x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
                 .AddFluentValidation(v =>
                 {

                     v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                 });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"), sqlOptions =>
        sqlOptions.MigrationsAssembly("CompanyApi"));
});

builder.Services.AddServicelayer();
builder.Services.AddRepositorylayer();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod());

app.MapControllers();

app.Run();
