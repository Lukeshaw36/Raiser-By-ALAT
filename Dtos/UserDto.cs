using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GROUP2.Dtos
{
    public class UserDto
    {
        [JsonIgnore] 
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(12, MinimumLength = 11, ErrorMessage = "Phone number must be between 11 and 12 characters")]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
