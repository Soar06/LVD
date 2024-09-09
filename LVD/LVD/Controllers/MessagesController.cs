using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LVD;
using LVD.Models;
using LVD.Models.DTO;

namespace LVD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ChatDbContext _context;
        private readonly GeneralFunction _generalFunction;

        public MessagesController(ChatDbContext context, GeneralFunction generalFunction)
        {
            _context = context;
            _generalFunction = generalFunction;
        }

        // GET: api/Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            return await _context.Messages.ToListAsync();
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Messages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, Message message)
        {
            if (id != message.MessageId)
            {
                return BadRequest();
            }

            _context.Entry(message).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Messages
        [HttpPost]
        public async Task<IActionResult> PostMessageWithSenderIdAndRoomId([FromBody] MessageDTO messageDto)
        {
            var message = new Message
            {
                MessageId = _generalFunction.GenerateUniqueId("m"), // Generate a unique MessageId
                Content = messageDto.Content,
                TimeSent = DateTime.UtcNow, // Set the TimeSent to the current time
                //IsEdited = false, // Default to false
                ChatRoomId = messageDto.ChatRoomId,
                UserId = messageDto.UserId
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok(messageDto);
        }

        // Get: api/Messages/5
        [HttpGet("latest/{groupId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetLatestMessages(int groupId)
        {
            var messages = await _context.Messages
                .Where(m => m.ChatRoomId == groupId)
                .OrderByDescending(m => m.TimeSent)
                .Take(30) // Take the latest 30 messages
                .ToListAsync();

            if (messages == null || !messages.Any())
            {
                return NotFound();
            }

            return Ok(messages);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.MessageId == id);
        }
    }
}
