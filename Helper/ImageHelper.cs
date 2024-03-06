namespace GROUP2.Helper
{
    public class ImageHelper
    {
        public static byte[] ReadImageFromFile(string imagePath)
        {
            try
            {
                if (File.Exists(imagePath))
                {
                    return File.ReadAllBytes(imagePath);
                }
                else
                {
                    // Handle the case where the image file does not exist
                    Console.WriteLine($"Image file not found: {imagePath}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                Console.WriteLine($"Error reading image file: {ex.Message}");
                return null;
            }
        }
    }
}
