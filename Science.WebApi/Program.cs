using Microsoft.EntityFrameworkCore;
using Science.Application;
using Science.Application.Common.Mappings;
using Science.Application.Interfaces;
using Science.Persistence;
using Science.WebApi.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Регистрируем AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(IExperimentsDbContext).Assembly));
});

// Добавляем слои приложения и инфраструктуры
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);

// Добавляем контроллеры
builder.Services.AddControllers();

// Добавляем генератор Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Инициализация базы данных при запуске приложения
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<ExperimentsDbContext>();
        context.Database.Migrate();
        DbInitializer.Initialize(context);
    }
    catch (Exception exception)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, "Ошибка при инициализации базы данных");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

// Включаем кастомную обработку исключений
app.UseCustomExceptionHandler();

// Middleware: маршрутизация, HTTPS, CORS
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Регистрируем маршруты контроллеров
app.MapControllers();

// Запуск приложения
app.Run();
