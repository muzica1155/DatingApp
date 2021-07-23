using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTo
    {
        [Required]//data anotation
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}