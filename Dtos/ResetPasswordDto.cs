using System.ComponentModel.DataAnnotations;

namespace GROUP2.Dtos
{
    public class ResetPasswordDto
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}

