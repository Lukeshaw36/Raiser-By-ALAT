namespace GROUP2.Helper
{
    public class CustomResetResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
      //  public UData  UserData { get; set; }



        public class UData
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            // You may exclude Password from being returned in the response for security reasons.
            // public string Password { get; set; }
        }


    }
}
