using central_informatica.src.Client.Components;
using central_informatica.src.Client.Services;
using central_informatica.src.Server.Repository;
using central_informatica.src.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProjectContext>(options =>
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("Main"))
        .EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine, LogLevel.Information)
);
builder.Services.AddScoped<central_informatica.src.Server.Services.PersonasService>();
builder.Services.AddSingleton<ToastService>();
builder.Services.AddHttpClient<central_informatica.src.Client.Services.PersonasService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5199/");
});

var app = builder.Build();

// Initialize static Toast class
using (var scope = app.Services.CreateScope())
{
    var toastService = scope.ServiceProvider.GetRequiredService<ToastService>();
    Toast.Initialize(toastService);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
