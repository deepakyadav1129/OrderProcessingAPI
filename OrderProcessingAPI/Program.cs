using Microsoft.EntityFrameworkCore;
using OrderProcessingAPI.Data;
using OrderProcessingAPI.Exceptions;
using OrderProcessingAPI.Repositories;
using OrderProcessingAPI.Services.Delivery;
using OrderProcessingAPI.Services.Discounts;
using OrderProcessingAPI.Services.Notifications;
using OrderProcessingAPI.Services.Orders;
using OrderProcessingAPI.Services.Payments;
using OrderProcessingAPI.Validators;
using System.Text.Json.Serialization;
namespace OrderProcessingAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                // API accepts and returns enum names such as "Card", "UPI", "COD".
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter(
                        namingPolicy: null,
                        allowIntegerValues: false));
            });

            builder.Services.AddOpenApi();

            // Configure centralized exception handling.
            builder.Services.AddProblemDetails();

            // Configure global exception handling.
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            // Configure SQL Server and EF Core.
            builder.Services.AddDbContext<OrderProcessingDbContext>(
                options =>
                {
                    options.UseSqlServer(
                        builder.Configuration.GetConnectionString("DefaultConnection"));
                });

            // Register repositories.
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register order validator.
            builder.Services.AddScoped<IOrderValidator, OrderValidator>();

            // Register all discount calculators.
            // IEnumerable<IDiscountCalculator> will receive all these implementations.
            builder.Services.AddScoped<IDiscountCalculator, RegularCustomerDiscountCalculator>();
            builder.Services.AddScoped<IDiscountCalculator, PremiumCustomerDiscountCalculator>();

            // Register all delivery-charge calculators.
            builder.Services.AddScoped<IDeliveryChargeCalculator, StandardDeliveryChargeCalculator>();
            builder.Services.AddScoped<IDeliveryChargeCalculator, ExpressDeliveryChargeCalculator>();
            builder.Services.AddScoped<IDeliveryChargeCalculator, SameDayDeliveryChargeCalculator>();

            // Register all payment processors.
            builder.Services.AddScoped<IPaymentProcessor, CardPaymentProcessor>();
            builder.Services.AddScoped<IPaymentProcessor, UpiPaymentProcessor>();
            builder.Services.AddScoped<IPaymentProcessor, CodPaymentProcessor>();

            // Register notification service.
            builder.Services.AddScoped<INotificationService, EmailNotificationService>();
            builder.Services.AddScoped<INotificationService, SmsNotificationService>();

            // Register order service.
            builder.Services.AddScoped<IOrderService, OrderService>();

            // Register payment service.
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            var app = builder.Build();

            // Configure OpenAPI / Swagger
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                        "/openapi/v1.json",
                        "Order Processing API v1");

                    options.RoutePrefix = "swagger";
                });
            }

            // Use centralized exception handling.
            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
