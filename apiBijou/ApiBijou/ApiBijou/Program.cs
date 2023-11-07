using ApiBijou.Model.Panier;
using ApiBijou.Model.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlX.XDevAPI;
using System;

var builder = WebApplication.CreateBuilder(args);

// Ajout des services � l'application
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache(); // Ajoute un cache en m�moire distribu�

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Temps d'inactivit� avant expiration de la session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//Ajout du cycle de vie Transient aux variables pour pouvoir faire une injection de d�pendance 
builder.Services.AddTransient<ApiBijou.Model.Session>();
builder.Services.AddTransient<PanierManager>();


var app = builder.Build();

// Configure le middleware de session
app.UseSession();

// Configurer le pipeline de requ�tes
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed(origin => true));

app.UseAuthorization();

app.MapControllers();

app.Run();
