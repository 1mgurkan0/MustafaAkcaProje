using StudentManagement.Data;
using StudentManagement.Data.Context;
using StudentManagement.Services.Extensions;
using StudentManagement.Services.Mapping;
using Serilog;
using Serilog.Events;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Web.Filters;
using StudentManagement.Data.Seeds;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/ubys-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("UBYS başlatılıyor...");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    var services = builder.Services;
    var config = builder.Configuration;
    var connString = config.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("DefaultConnection tanımlanmamış.");

    services.AddControllersWithViews();
    services.AddDbContext<ApplicationDbContext>(opts =>
        opts.UseSqlServer(connString, sql => sql.MigrationsAssembly("StudentManagement.Data")));

    services.AddDistributedMemoryCache();
    services.AddSession(opts =>
    {
        opts.IdleTimeout = TimeSpan.FromMinutes(30);
        opts.Cookie.HttpOnly = true;
        opts.Cookie.IsEssential = true;
        opts.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        opts.Cookie.SameSite = SameSiteMode.Strict;
        opts.Cookie.Name = "UBYS.Session";
    });

    services.AddHttpContextAccessor();
    services.AddAutoMapper(typeof(MappingProfile));
    services.AddScoped<SessionAuthFilter>();
    services.AddDataServices(connString);
    services.AddUbysServices();
    services.AddUbysValidators();

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }
    else
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseMiddleware<StudentManagement.Web.Middlewares.GlobalExceptionMiddleware>();
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseSession();
    app.UseMiddleware<StudentManagement.Web.Middlewares.SessionValidationMiddleware>();

    app.MapControllerRoute(name: "admin", pattern: "Admin/{action=Dashboard}/{id?}", defaults: new { controller = "Admin" });
    app.MapControllerRoute(name: "ogretmenPanel", pattern: "OgretmenPanel/{action=Dashboard}/{id?}", defaults: new { controller = "OgretmenPanel" });
    app.MapControllerRoute(name: "ogrenciPanel", pattern: "OgrenciPanel/{action=Dashboard}/{id?}", defaults: new { controller = "OgrenciPanel" });
    app.MapControllerRoute(name: "ogrenciIsleri", pattern: "OgrenciIsleri/{action=Dashboard}/{id?}", defaults: new { controller = "OgrenciIsleriPanel" });
    app.MapControllerRoute(name: "default", pattern: "{controller=Auth}/{action=Login}/{id?}");

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Veritabanı migrate ediliyor...");
            await db.Database.MigrateAsync();
            logger.LogInformation("Seed verisi kontrol ediliyor...");
            // Burada scope çakışması vardı, sildik ve doğrudan db'yi yolladık.
            await DataSeeder.SeedAsync(db);
            logger.LogInformation("Veritabanı hazır.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "DB migrate/seed sırasında hata oluştu.");
        }
    }

    Log.Information("UBYS çalışıyor → http://localhost:5000");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "UBYS başlatılamadı.");
}
finally
{
    Log.CloseAndFlush();
}