using Microsoft.EntityFrameworkCore;
using SnackBar.MessageBus;
using SnackBar.Services.Email.DbContexts;
using SnackBar.Services.Email.Extensions;
using SnackBar.Services.Email.Messaging;
using SnackBar.Services.Email.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//configure auto mapper
//  IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
//builder.Services.AddSingleton(mapper);
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//configure database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//configure DbContextOptions as DI for OrderRepo
var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(new EmailRepository(optionsBuilder.Options));

//add OrderRepo as DI
//builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

//Add Azure Service Bus Consumer to DI
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

//addAzure ServiceBusMessageBuilder/Sender to dependency Injection to publish the message in AzureServiceBus
builder.Services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();

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

app.UseAzureServiceBusConsumer();

app.Run();
