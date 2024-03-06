namespace GROUP2.Helper
{
    public class PasswordValidator
    {
        public static bool IsStrongPassword(string password)
        {
            // Check for minimum length
            if (password.Length < 8)
                return false;

            // Check for at least one uppercase letter
            if (!password.Any(char.IsUpper))
                return false;

            // Check for at least one lowercase letter
            if (!password.Any(char.IsLower))
                return false;

            // Check for at least one digit
            if (!password.Any(char.IsDigit))
                return false;

            // Check for at least one special character
            if (!password.Any(c => !char.IsLetterOrDigit(c)))
                return false;

            // If all criteria are met, consider it a strong password
            return true;
        }


        public static bool ArePasswordsValid(string password, string confirmPassword)
        {
            // Check if passwords match
            //bool passwordsMatch = password == confirmPassword;

            // Check if both passwords are strong
            bool arePasswordsStrong = IsStrongPassword(password) && IsStrongPassword(confirmPassword);

            // Return true if both passwords are strong
            return  arePasswordsStrong;
        }
    }
}
