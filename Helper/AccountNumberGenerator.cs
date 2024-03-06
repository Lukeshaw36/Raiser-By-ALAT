namespace GROUP2.Helper
{
    public class AccountNumberGenerator
    {
        private static readonly Random _random = new Random();

        //public static string GenerateAccountNumber(int length)
        //{
        //    const string chars = "0123456789";
        //    return new string(Enumerable.Repeat(chars, length)
        //      .Select(s => s[_random.Next(s.Length)]).ToArray());
        //}
        public static string GenerateAccountNumber(int length)
        {
            const string chars = "0123456789";
            // Generate a string of random digits except for the first digit, which is forced to be '0'
            string accountNumber = $"0{new string(Enumerable.Repeat(chars, length - 1).Select(s => s[_random.Next(s.Length)]).ToArray())}";
            return accountNumber;
        }

    }
}
