var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Lista em memória para armazenar os carros
List<Car> cars = new List<Car>
{
    new Car { Id = 1, Model = "Carro 1", Description = "Descrição do Carro 1", Price = 10000.99m, Quantity = 10 },
    new Car { Id = 2, Model = "Carro 2", Description = "Descrição do Carro 2", Price = 20000.49m, Quantity = 5 },
    new Car { Id = 3, Model = "Carro 3", Description = "Descrição do Carro 3", Price = 15000.79m, Quantity = 20 }
};

// Rota GET para obter todos os carros
app.MapGet("/cars", () => cars);

// Rota GET para obter um carro pelo Id
app.MapGet("/cars/{id}", (int id) =>
{
    var car = cars.FirstOrDefault(c => c.Id == id);
    if (car == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(car);
});

// Rota POST para criar um novo carro
app.MapPost("/cars", (Car car) =>
{
    car.Id = cars.Count + 1;
    cars.Add(car);
    return Results.Created($"/cars/{car.Id}", car);
});

// Rota PUT para atualizar um carro existente
app.MapPut("/cars/{id}", (int id, Car updatedCar) =>
{
    var existingCar = cars.FirstOrDefault(c => c.Id == id);
    if (existingCar == null)
    {
        return Results.NotFound();
    }

    existingCar.Model = updatedCar.Model;
    existingCar.Description = updatedCar.Description;
    existingCar.Price = updatedCar.Price;
    existingCar.Quantity = updatedCar.Quantity;

    return Results.Ok(existingCar);
});

// Rota DELETE para excluir um carro
app.MapDelete("/cars/{id}", (int id) =>
{
    var existingCar = cars.FirstOrDefault(c => c.Id == id);
    if (existingCar == null)
    {
        return Results.NotFound();
    }

    cars.Remove(existingCar);
    return Results.NoContent();
});

app.Run();

// Definição da classe de Carro
public class Car
{
    public int Id { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
