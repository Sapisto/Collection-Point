using CollectPoint.Infrastructure.Extension;

var builder = WebApplication.CreateBuilder(args);

// ---- Services: what can be injected (Nest module providers) ----
builder.Services.AddDatabaseContext(builder.Configuration);
builder.Services.AddScopedServices();
builder.Services.AddRepositoryServices();
builder.Services.AddCustomControllers();
builder.Services.AddSwaggerOpenAPI();

var app = builder.Build();

// ---- Pipeline: ORDER MATTERS, like app.use() in Express ----
app.ConfigureCustomExceptionMiddleware(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();      // → /swagger
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
