using ProductosApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Se registra una sola clase para ambas interfaces
builder.Services.AddScoped<IProductoBusqueda, ProductoRepository>();
builder.Services.AddScoped<IProductoRepository>(sp => sp.GetRequiredService<IProductoBusqueda>());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();