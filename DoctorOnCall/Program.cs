using DoctorOnCall;
using DoctorOnCall.Extensions;
using DoctorOnCall.Models;
using DoctorOnCall.Utils;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:5173"));

app.UseAuthentication();
app.UseAuthorization();


app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();