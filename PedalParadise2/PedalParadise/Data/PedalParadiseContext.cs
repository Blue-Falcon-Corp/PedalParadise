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
            
            //configure Discriminators for TPH Mappping
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<User>("User")
                .HasValue<Employee>("Employee")
                .HasValue<Client>("Client");

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

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.ManagerNavigation)
                .WithMany(e => e.ManagedEmployees)
                .HasForeignKey(e => e.Manager)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Client entity
            modelBuilder.Entity<Client>()
                .Property(c => c.Membership)
                .HasMaxLength(20)
                .IsRequired(); // Ensure Membership is constrained properly
                       
            // Define relationships for collections
            modelBuilder.Entity<Client>()
                .HasMany(c => c.ShoppingCarts)
                .WithOne(s => s.Client)
                .HasForeignKey(s => s.UserID) //Use UserID instead of ClientID
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Client)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Reviews)
                .WithOne(r => r.Client)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.RepairRequests)
                .WithOne(rr => rr.Client)
                .HasForeignKey(rr => rr.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}