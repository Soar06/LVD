using System.ComponentModel.DataAnnotations;

namespace LVD.Models
{
    public class ChatRoom
    {
        [Key]
        public int ChatID { get; set; }
        public string ChatRoomName { get; set; }
        public string? Password { get; set; }

        public ChatRoom()
        {
            this.Messages = new HashSet<Message>();
            this.Users = new HashSet<User>();
        }

        public ICollection<Message> Messages { get; set; } 
        public ICollection<User> Users { get; set; }
    }
}
