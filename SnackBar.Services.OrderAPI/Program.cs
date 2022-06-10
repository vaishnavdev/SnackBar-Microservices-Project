using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SnackBar.MessageBus;
using SnackBar.Services.OrderAPI.DbContexts;
using SnackBar.Services.OrderAPI.Extensions;
using SnackBar.Services.OrderAPI.Messaging;
using SnackBar.Services.OrderAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//configure auto mapper
//  IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
//builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//configure DbContextOptions as DI for OrderRepo
var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(new OrderRepository(optionsBuilder.Options));

//add OrderRepo as DI
//builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

//Add Azure Service Bus Consumer to DI
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SnackBar.Services.CouponApi", Version = "v1" });
    //c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = @"Enter 'Bearer' [space] and your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    //format that is needed to enable security requirement in Swagger Gen
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header
        },
        new List<string>()
        }
    });
});

//configure database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Add authentication to use IdentityServer
builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://localhost:44352";
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = false
    };
});

//Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "snackbar");

    });
});

//addAzure ServiceBusMessageBuilder/Sender to dependency Injection to publish the message in AzureServiceBus
builder.Services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//the extension method that we have created,
//and it will start and stop the processing i,e consuming the message from AzureServiceBus
app.UseAzureServiceBusConsumer();

app.Run();
