using JuanApp.Application;
using JuanApp.MVC;
using JuanApp.Persistance;
using JuanApp.Persistance.DAL.Context;
using JuanApp.Persistance.DAL.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();       
builder.Services.AddPersistenceServices(builder.Configuration); 
builder.Services.AddMVCServices();                 

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var dbContext = services.GetRequiredService<JuanAppContext>();

//    await dbContext.Database.MigrateAsync();

//    await IdentitySeeder.SeedRolesAndAdminAsync(services);
//}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/HttpStatusCodeHandler");
}
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
