using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Hephaistos_Maui.Services
{
    public static class AuthService
    {
        private static readonly string ApiBaseUrl = "https://hephaistos-backend-c6c5ewhraedvgzex.germanywestcentral-01.azurewebsites.net/api";

        // OTP kérés
        public static async Task<bool> RequestOtpAsync(string email)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = $"{ApiBaseUrl}/requestOtp";
                    var content = new StringContent(JsonSerializer.Serialize(new { email }), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(url, content);
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }

        // OTP ellenőrzés és jelszó változtatás
        public static async Task<bool> VerifyOtpAsync(string email, string otp, string newPassword)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = $"{ApiBaseUrl}/verifyOtp";
                    var content = new StringContent(JsonSerializer.Serialize(new { email, otp, newPassword }), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(url, content);
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
