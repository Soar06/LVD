using System.Collections.Generic;
using System.Threading.Tasks;
using LVD.Models;
using LVD.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace LVD.Services
{
    public interface IChatRoomsService
    {
        Task<IEnumerable<ChatRoom>> GetChatRoomsAsync();
        Task<ChatRoom> GetChatRoomAsync(int id);
        Task<IActionResult> UpdateChatRoomAsync(int id, ChatRoom chatRoom);
        Task<ActionResult<ChatRoomDTO>> CreateChatRoomAsync(ChatRoomDTO chatRoomDto);
        Task<IActionResult> DeleteChatRoomAsync(int id);
    }
}
