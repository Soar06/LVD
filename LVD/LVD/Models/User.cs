using System.ComponentModel.DataAnnotations;

namespace LVD.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public UserStatus Status { get; set; }
        public UserType Type { get; set; }
        public User()
        {
            this.Messages = new HashSet<Message>();
            this.ChatRooms = new HashSet<ChatRoom>();
        }

        public ICollection<Message> Messages { get; set; }
        public ICollection<ChatRoom> ChatRooms { get; set; }

        public enum UserStatus
        {
            Online,
            Offline
        }

        public enum UserType
        {
            Seller,
            Client
        }

    }
}
