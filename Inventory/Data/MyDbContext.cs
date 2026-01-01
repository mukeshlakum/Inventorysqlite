using Inventory.Entities;
using Microsoft.EntityFrameworkCore;
namespace Inventory.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext>options):base(options) {}
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Item> Items { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().HasData
        //        (new User() { Id = new Guid, Username = "NewName", PasswordHash = new byte[] { "dkdkdk" });

        //    base.OnModelCreating(modelBuilder); 
        //}
    }
}
