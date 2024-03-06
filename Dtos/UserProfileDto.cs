using GROUP2.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GROUP2.Dtos
{
    public class UserProfileDto
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public string AccountNumber { get; set; }

    }

    public class UpdateUserProfileDto
    {
        public int UserId { get; set; } 
        public string? Experience { get; set; }
        public string? Status { get; set; }
        public string? Income { get; set; }

        //public string? Interest { get; set; }
        //public List<InvestmentInterestRequest> InvestmentInterests { get; set; }
    }

    public class InvestmentInterestRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
