using LVD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LVD
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseMySQL("YourConnectionStringHere");

            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the many-to-many relationship between User and ChatRoom
            modelBuilder.Entity<User>()
                .HasMany(u => u.ChatRooms)
                .WithMany(c => c.Users)
                .UsingEntity(j => j.ToTable("UserChatRooms")); // Customize the join table name

            // Configuring the one-to-many relationship between Message and ChatRoom
            modelBuilder.Entity<Message>()
                .HasOne(m => m.ChatRoom)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatRoomId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Set cascade delete behavior

            // Configuring the one-to-many relationship between Message and User
            modelBuilder.Entity<Message>() 
                .HasOne(m => m.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Set cascade delete behavior
        }

    }
}
