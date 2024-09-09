using static LVD.Models.User;

namespace LVD.Models.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ProfilePictureUrl { get; set; }
        public User.UserStatus Status { get; set; }
        public User.UserType Type { get; set; }
    }

}
