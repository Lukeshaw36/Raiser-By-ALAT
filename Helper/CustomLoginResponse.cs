namespace GROUP2.Helper
{
    public class CustomLoginResponse
    {
        public class LoginResponse
        {
            public int UserId { get; set; } 
            public string Token { get; set; }
            public string Message { get; set; }
            public bool Success { get; set; }
            public Detail UserDetails { get; set; }
        }

        public class Detail
        {
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            // You may exclude Password from being returned in the response for security reasons.
            // public string Password { get; set; }
        }
    }
}
