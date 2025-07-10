using System.Numerics;

namespace localMarketingSystem.DTOs
{
    public class CreateUserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }
    }
    public class UserListDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? MobileNo { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }
    }

    public class UpdateUserDTO 
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }
    }
}
