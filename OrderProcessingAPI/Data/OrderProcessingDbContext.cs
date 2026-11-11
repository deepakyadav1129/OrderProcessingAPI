using Microsoft.EntityFrameworkCore;
using OrderProcessingAPI.Entities;
using OrderProcessingAPI.Enums;

namespace OrderProcessingAPI.Data
{
    public class OrderProcessingDbContext : DbContext
    {
        public OrderProcessingDbContext(
            DbContextOptions<OrderProcessingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureCustomer(modelBuilder);
            ConfigureProduct(modelBuilder);
            ConfigureOrder(modelBuilder);
            ConfigureOrderItem(modelBuilder);
            ConfigurePayment(modelBuilder);

            SeedData(modelBuilder);
        }

        private static void ConfigureCustomer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .Property(c => c.Email)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .Property(c => c.MobileNumber)
                .HasMaxLength(20)
                .IsRequired();

            // CustomerType is an enum in C#,
            // but stored as a string in SQL Server.
            modelBuilder.Entity<Customer>()
                .Property(c => c.CustomerType)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();
        }

        private static void ConfigureProduct(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
        }

        private static void ConfigureOrder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(o => o.SubTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.DiscountAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.DeliveryCharge)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            // Store OrderStatus enum as text.
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .Property(o => o.DeliveryMethod)
                .HasMaxLength(50)
                .IsRequired();

            // Store PaymentMethod enum as text.
            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentMethod)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureOrderItem(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.LineTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigurePayment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            // Store PaymentMethod enum as text.
            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentMethod)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            // TransactionId is nullable because COD
            // has no transaction during order creation.
            modelBuilder.Entity<Payment>()
                .Property(p => p.TransactionId)
                .HasMaxLength(100);

            // Store PaymentStatus enum as text.
            modelBuilder.Entity<Payment>()
                .Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            // Initial customers used for testing.
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    Name = "Amit Kumar",
                    Email = "amit@example.com",
                    MobileNumber = "9876543210",
                    CustomerType = CustomerType.Regular
                },
                new Customer
                {
                    Id = 2,
                    Name = "Priya Sharma",
                    Email = "priya@example.com",
                    MobileNumber = "9876543211",
                    CustomerType = CustomerType.Premium
                },
                new Customer
                {
                    Id = 3,
                    Name = "Rahul Verma",
                    Email = "rahul@example.com",
                    MobileNumber = "9876543212",
                    CustomerType = CustomerType.Regular
                }
            );

            // Initial products used for testing.
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Price = 65000,
                    StockQuantity = 10
                },
                new Product
                {
                    Id = 2,
                    Name = "Wireless Mouse",
                    Price = 1500,
                    StockQuantity = 30
                },
                new Product
                {
                    Id = 3,
                    Name = "Mechanical Keyboard",
                    Price = 3500,
                    StockQuantity = 20
                },
                new Product
                {
                    Id = 4,
                    Name = "24 Inch Monitor",
                    Price = 18000,
                    StockQuantity = 15
                },
                new Product
                {
                    Id = 5,
                    Name = "Bluetooth Headphones",
                    Price = 2500,
                    StockQuantity = 25
                }
            );
        }
    }
}
