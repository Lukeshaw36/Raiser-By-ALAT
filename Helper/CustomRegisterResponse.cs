namespace GROUP2.Helper
{
    public class CustomRegisterResponse
    {
        public class CustomNewResponse
        {
            
            public string Message { get; set; }
            public bool Success { get; set; }
            //public UserDetail UserDetails { get; set; }
        }


        public class UserDetail
        {
              public int UserId { get; set; } 
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            
        }
    }
}
