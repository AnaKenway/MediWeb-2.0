using DataLayer;
using MediWeb.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString") ?? throw new InvalidOperationException("Connection string 'DefaultConnectionString' not found.");
builder.Services.AddDbContext<MediWebContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<UserAccount>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<MediWebContext>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

#region Services
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<ClinicService>();
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<AppointmentSlotService>();
builder.Services.AddScoped<DoctorClinicsService>();
builder.Services.AddScoped<MedicalEmployeeService>();
builder.Services.AddScoped<SpecializationService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
