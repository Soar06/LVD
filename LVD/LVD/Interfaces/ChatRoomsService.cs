using Microsoft.EntityFrameworkCore;
using LVD.Models;
using LVD.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LVD.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace LVD.Services
{
    public class ChatRoomsService : IChatRoomsService
    {
        private readonly ChatDbContext _context;
        private readonly GeneralFunction _generalFunction;

        public ChatRoomsService(ChatDbContext context, GeneralFunction generalFunction)
        {
            _context = context;
            _generalFunction = generalFunction;
        }

        public async Task<IEnumerable<ChatRoom>> GetChatRoomsAsync()
        {
            return await _context.ChatRooms.ToListAsync();
        }

        public async Task<ChatRoom> GetChatRoomAsync(int id)
        {
            return await _context.ChatRooms.FindAsync(id);
        }

        public async Task<IActionResult> UpdateChatRoomAsync(int id, ChatRoom chatRoom)
        {
            if (id != chatRoom.ChatID)
            {
                return new BadRequestResult();
            }

            _context.Entry(chatRoom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatRoomExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new NoContentResult();
        }

        public async Task<ActionResult<ChatRoomDTO>> CreateChatRoomAsync(ChatRoomDTO chatRoomDto)
        {
            var chatRoom = new ChatRoom
            {
                ChatID = _generalFunction.GenerateUniqueId("cr"),
                ChatRoomName = chatRoomDto.ChatRoomName
            };

            _context.ChatRooms.Add(chatRoom);
            await _context.SaveChangesAsync();

            if (chatRoomDto.UserIds != null && chatRoomDto.UserIds.Count > 0)
            {
                var users = await _context.Users
                    .Where(u => chatRoomDto.UserIds.Contains(u.UserId))
                    .ToListAsync();

                if (users.Count != chatRoomDto.UserIds.Count)
                {
                    return new BadRequestObjectResult("One or more users not found.");
                }

                foreach (var user in users)
                {
                    if (!user.ChatRooms.Contains(chatRoom))
                    {
                        user.ChatRooms.Add(chatRoom);
                    }
                }

                _context.Entry(chatRoom).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            var createdChatRoomDto = new ChatRoomDTO
            {
                ChatID = chatRoom.ChatID,
                ChatRoomName = chatRoom.ChatRoomName,
                UserIds = chatRoomDto.UserIds
            };

            return new CreatedAtActionResult(nameof(GetChatRoomAsync), "ChatRooms", new { id = chatRoom.ChatID }, createdChatRoomDto);
        }

        public async Task<IActionResult> DeleteChatRoomAsync(int id)
        {
            var chatRoom = await _context.ChatRooms.FindAsync(id);
            if (chatRoom == null)
            {
                return new NotFoundResult();
            }

            _context.ChatRooms.Remove(chatRoom);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        private bool ChatRoomExists(int id)
        {
            return _context.ChatRooms.Any(e => e.ChatID == id);
        }
    }
}
