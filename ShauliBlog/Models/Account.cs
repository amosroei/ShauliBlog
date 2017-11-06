
using System.ComponentModel.DataAnnotations;


namespace ShauliBlog.Models
{
    public class Account
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Please confirm your Password")]
        [DataType(DataType.Password)]
        public string ComfirmPassword { get; set; }
        public string Website { get; set; }
        public bool IsAdmin { get; set; }
    }
}