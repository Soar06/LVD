using System.ComponentModel.DataAnnotations.Schema;

namespace LVD.Models.DTO
{
    public class MessageDTO
    {
        public string Content { get; set; }
        public int ChatRoomId { get; set; }
        public int UserId { get; set; }

    }
}
