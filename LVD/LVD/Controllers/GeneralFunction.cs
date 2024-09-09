using LVD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace LVD.Controllers
{
    public class GeneralFunction
    {
        private readonly ChatDbContext _context;

        public GeneralFunction(ChatDbContext context)
        {
            _context = context;
        }

        public bool IsIdExists(int id, string x)
        {
            return x switch
            {
                "u" => _context.Users.Any(e => e.UserId == id),
                "cr" => _context.ChatRooms.Any(e => e.ChatID == id),
                "m" => _context.Messages.Any(e => e.MessageId == id),
                _ => false
            };
        }

        public int GenerateUniqueId(string x)
        {
            Random rnd = new Random();
            int id;

            do
            {
                id = rnd.Next(100000, 1000000); ; // Generate a random integer
            } while (IsIdExists(id, x));

            return id;
        }
        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("InBrightestDayInBlackestNightNoEvilShallEscapeMySightLetThoseWhoWorshipEvilsMightBewareMyPowerGreenLanternsLight"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7177/auth",
                audience: "https://localhost:7177/api",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }


}
