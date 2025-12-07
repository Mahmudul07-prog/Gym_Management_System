using BLL.Profiles;
using BLL.Services.AttachmentService;
using BLL.Services.Classes;
using BLL.Services.Interfaces;
using DataAccess.Data.Contexts;
using DataAccess.Data.DataSeed;
using DataAccess.Models;
using DataAccess.Repositroies.Classes;
using DataAccess.Repositroies.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // DI
            builder.Services.AddDbContext<GymSystemDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                //options.UseSqlServer(builder.Configuration["ConnectionString:DefaultConnection"]);
            });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IMemberShipRepository, MemberShipRepository>();
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped<ISessionScheduleRepository, SessionScheduleRepository>();
            builder.Services.AddAutoMapper(X => X.AddProfile(new MappingProfile()));

            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IMemberShipService, MemberShipService>();
            builder.Services.AddScoped<ISessionScheduleService, SessionScheduleService>();


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<GymSystemDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            #endregion

            var app = builder.Build();

            #region Data Seed
            var Scope = app.Services.CreateScope();
            var dbContext = Scope.ServiceProvider.GetRequiredService<GymSystemDbContext>(); // obj DbContext

            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations?.Any() ?? false)
                dbContext.Database.Migrate();

            Seeding.SeedData(dbContext);

            // =========
            var RoleManager = Scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = Scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            IdentityDBContextSeeding.SeedData(RoleManager, UserManager);
            #endregion


            #region PipeLine
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            #endregion

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
