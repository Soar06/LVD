namespace LVD.Models.DTO
{
    public class ChatRoomDTO
    {
            public int ChatID { get; set; }
            public string ChatRoomName { get; set; }
            public List<int> UserIds { get; set; } // List of UserIds to associate with the chat room

    }
}
