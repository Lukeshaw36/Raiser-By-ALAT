using System.ComponentModel.DataAnnotations;

namespace GROUP2.Dtos
{
    public class PasswordChangeDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
