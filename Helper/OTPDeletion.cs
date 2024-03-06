//using Microsoft.Data.SqlClient;

//namespace GROUP2.Helper
//{
//    public class OTPDeletion
//    {
      
//        public static string connectionString = "Server=tcp:bit-group-one-back-end-server.database.windows.net,1433;Initial Catalog=dbTest;Persist Security Info=False;User ID=Bitgroupone;Password=Devops@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;";
 
//        public  static bool DeleteOTP(string? otp)
//        {
//            bool success = false;
//            try
//            {
//                using (SqlConnection connection = new SqlConnection(connectionString))
//                {
//                    connection.Open();

//                    string deleteQuery = "UPDATE Users SET OTP = NULL WHERE OTP = @OTP";

//                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
//                    {
//                        command.Parameters.AddWithValue("@OTP", otp);
//                        int rowsAffected = command.ExecuteNonQuery();
//                        if (rowsAffected > 0)
//                        {
//                            success = true;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Error deleting OTP: " + ex.Message);
//                // You might want to handle this error more gracefully, perhaps logging it
//            }
//            return success;
//        }

//    }
//}
