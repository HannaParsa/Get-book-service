using taaghche;
using taaghche.RabitMQ;
using taaghche.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
// add service
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ITaghcheService, TaghcheService>();
builder.Services.AddScoped<IInMemoryCacheService, InMemoryCacheService>();
builder.Services.AddScoped<IDistributedCacheService, RedisCacheService>();
builder.Services.AddScoped<IRabitMQCunsomer, RabitMQCunsomer>();

//add to memory
builder.Services.AddMemoryCache();

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

app.Run();
