using System.ComponentModel.DataAnnotations;

namespace GROUP2.Dtos
{
    public class ResendOtpDto
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
//updating development branch