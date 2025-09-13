using Microsoft.EntityFrameworkCore;
using Science.Application;
using Science.Application.Common.Mappings;
using Science.Application.Interfaces;
using Science.Persistence;
using Science.WebApi.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ������������ AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(IExperimentsDbContext).Assembly));
});

// ��������� ���� ���������� � ��������������
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);

// ��������� �����������
builder.Services.AddControllers();

// ��������� ��������� Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ������������� ���� ������ ��� ������� ����������
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
        logger.LogError(exception, "������ ��� ������������� ���� ������");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

// �������� ��������� ��������� ����������
app.UseCustomExceptionHandler();

// Middleware: �������������, HTTPS, CORS
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

// ������������ �������� ������������
app.MapControllers();

// ������ ����������
app.Run();
