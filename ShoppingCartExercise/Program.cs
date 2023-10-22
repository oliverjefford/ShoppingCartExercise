using ShoppingCartExercise.DatabaseContext;
using ShoppingCartExercise.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ShoppingCartDatabaseContext>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IOfferRepository, OfferRepository>();
builder.Services.AddTransient<IBasketRepository, BasketRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
