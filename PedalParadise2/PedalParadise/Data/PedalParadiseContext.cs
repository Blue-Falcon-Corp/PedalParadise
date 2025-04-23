using Microsoft.EntityFrameworkCore;
using PedalParadise.Models;

namespace PedalParadise.Data
{
    public class PedalParadiseContext : DbContext
    {
        public PedalParadiseContext(DbContextOptions<PedalParadiseContext> options) : base(options)
        {
            Users = Set<User>();
            Employees = Set<Employee>();
            Clients = Set<Client>();
            Products = Set<Product>();
            ShoppingCarts = Set<ShoppingCart>();
            CartItems = Set<CartItem>();
            PaymentMethods = Set<PaymentMethod>();
            Orders = Set<Order>();
            OrderItems = Set<OrderItem>();
            Reviews = Set<Review>();
            RepairRequests = Set<RepairRequest>();
            RefundRequests = Set<RefundRequest>();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<RepairRequest> RepairRequests { get; set; }
        public DbSet<RefundRequest> RefundRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure CartItem composite key
            modelBuilder.Entity<CartItem>()
                .HasKey(ci => new { ci.CartID, ci.ProductID });

            // Configure CartItem relationships
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(sc => sc.CartItems)
                .HasForeignKey(ci => ci.CartID);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductID);

            // Configure OrderItem composite key
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderID, oi.ProductID });

            // Configure OrderItem relationships
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderID);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductID);
        }
    }
}