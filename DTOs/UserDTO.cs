using System.ComponentModel.DataAnnotations;

namespace ElyessLink_API.DTOs
{
    public class UserGetDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePicturePath { get; set; }
    }


    public class UserCreateDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class UserAuthDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
