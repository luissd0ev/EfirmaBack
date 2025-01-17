using APIEfirma.Interfaces;
using APIEfirma.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<StorageService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
   builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().AllowAnyOrigin().WithOrigins("http://localhost", "http://193.168.2.36", "http://26.36.60.51", "http://localhost:4200","https://siacetest.azurewebsites.net", "https://sistemaa3.azurewebsites.net");
}));
builder.Services.AddScoped<ICertificadoService, CertificadoService>();
builder.Services.AddScoped<IFirmaService, FirmaService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("corsapp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
