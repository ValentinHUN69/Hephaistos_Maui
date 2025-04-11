using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;  // System.Text.Json importálása

namespace Hephaistos_Maui.Models
{
    public static class ChangePasswordApi
    {
        // Az API base URL-je
        private static readonly string ApiBaseUrl = "https://hephaistos-backend-c6c5ewhraedvgzex.germanywestcentral-01.azurewebsites.net/api";

        // A jelszó változtatásához tartozó végpont
        private static readonly string ChangePasswordEndpoint = "/change-password";

        // Jelszó változtatásának aszinkron metódusa
        public static async Task<bool> ChangePasswordAsync(ChangePasswordModel model)
        {
            try
            {
                // HttpClient példány létrehozása
                using (var httpClient = new HttpClient())
                {
                    // Az API végpont URL-jének összeállítása
                    var url = ApiBaseUrl + ChangePasswordEndpoint;

                    // A modell JSON formátumban történő szerializálása System.Text.Json használatával
                    var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

                    // POST kérés küldése az API végpontra
                    var response = await httpClient.PostAsync(url, content);

                    // Ellenőrizzük, hogy a válasz sikeres volt-e
                    if (response.IsSuccessStatusCode)
                    {
                        // A válasz sikeres, térjünk vissza true-val
                        return true;
                    }
                    else
                    {
                        // Ha a válasz nem sikeres, logolj egy hibát és térj vissza false-szal
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Ha bárminemű hiba történik (pl. hálózati hiba), logolj és térj vissza false-szal
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }
    }
}
