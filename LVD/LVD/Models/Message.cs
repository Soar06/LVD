using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LVD.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public string Content { get; set; }
        public DateTime TimeSent { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? EditedTime { get; set; }

        [ForeignKey("ChatRoom")]
        public int ChatRoomId { get; set; } // Foreign key to ChatRoom
        public virtual ChatRoom ChatRoom { get; set; } // Navigation property

        [ForeignKey("User")]
        public int UserId { get; set; } // Foreign key to User
        public virtual User Sender { get; set; }
    }

}
