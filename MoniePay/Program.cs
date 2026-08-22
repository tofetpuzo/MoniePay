// SPDX-License-Identifier: Apache-2.0
/*
 * Application composition root and HTTP pipeline.
 *
 * Copyright (c) 2026, MoniePay
 */

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoniePay.Components;
using MoniePay.src.auth;
using MoniePay.src.data;
using MoniePay.src.services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<IPaymentIntentService, PaymentIntentService>();
builder.Services.AddScoped<LedgerAccountService>();

builder.Services.AddDbContextPool<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Without this nothing validated the JWTs IdentityService was minting, so no
// ClaimsPrincipal was ever built and [Authorize] could not work. Values must
// match the ones IdentityService signs with (JwtSettings:*).
var jwtSecret = builder.Configuration["JwtSettings:SecretKey"]
    ?? throw new InvalidOperationException("JwtSettings:SecretKey is not configured");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),

            // JwtBearer otherwise remaps role claims to a long schema URI and
            // [Authorize(Roles = "Admin")] silently stops matching. RoleFlags
            // emits plain ClaimTypes.Role, so pin both ends.
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.NameIdentifier,

            // Default is 5 minutes, which lets expired tokens through.
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

// Order matters: authentication builds the principal, authorization checks it.
// Both must sit before MapControllers or [Authorize] is never evaluated.
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MoniePay.Client._Imports).Assembly);

app.MapControllers();

app.Run();
