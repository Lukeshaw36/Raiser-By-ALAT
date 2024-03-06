using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GROUP2.Models
{
    public class User
    {
        [JsonIgnore]
        public int Id { get; set; } 
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
      
        public decimal WalletBalance { get; set; }


        public string? AccountNumber { get; set; }   

        public string?
            OTP  {get; set; }

       // public string? OTP {  get; set; }

        public decimal Balance { get; set; }


        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiration { get; set; }


        public bool HasLoggedIn { get; set; }





        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }


        public string? Experience { get; set; }
        public string? Status { get; set; }
        public string? Income { get; set; }

        //public string? Interest { get; set; }

        //public List<InvestmentInterest>? InvestmentInterests { get; set; }


        public List<Investment>? InvestmentInterests { get; set; }
 
    }
}
