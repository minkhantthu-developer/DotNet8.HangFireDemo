using Hangfire;
using HangfireBasicAuthenticationFilter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DbConnection"));
});
builder.Services.AddHangfireServer();

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

app.UseHangfireDashboard("/hangfire",new DashboardOptions
{
    DashboardTitle="HangFireDemo",
    DarkModeEnabled=false,
    DisplayStorageConnectionString=false,
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            User="Minkhant",
            Pass="HninEi"
        }
    }
});

app.Run();
