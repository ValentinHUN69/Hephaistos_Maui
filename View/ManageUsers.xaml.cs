using Hephaistos_Maui.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AppMajor = Hephaistos_Maui.Models.Major;


namespace Hephaistos_Maui.View;

public partial class ManageUsersPage : ContentPage
{
    private readonly string apiBaseUrl = "https://hephaistos-backend-c6c5ewhraedvgzex.germanywestcentral-01.azurewebsites.net/api";

    public ObservableCollection<User> Users { get; set; } = new();
    public ObservableCollection<University> Universities { get; set; } = new();
    public ObservableCollection<AppMajor> AvailableMajors { get; set; } = new();

    public User SelectedUser { get; set; } = new();
    public string SelectedUniversityId { get; set; } = string.Empty;

    public ManageUsersPage()
    {
        InitializeComponent();
        BindingContext = this;
        LoadUsers();
        LoadUniversities();
    }

    private async void LoadUsers()
    {
        try
        {
            using HttpClient client = new();
            string token = await SecureStorage.GetAsync("accessToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetStringAsync($"{apiBaseUrl}/User/admin/all");
            var result = JsonSerializer.Deserialize<ApiListResponse<User>>(response);
            Users.Clear();
            foreach (var u in result?.Values ?? new List<User>())
                Users.Add(u);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hiba", $"Nem sikerült a felhasználókat betölteni: {ex.Message}", "OK");
        }
    }

    private async void LoadUniversities()
    {
        try
        {
            using HttpClient client = new();
            string token = await SecureStorage.GetAsync("accessToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetStringAsync($"{apiBaseUrl}/University");
            var list = JsonSerializer.Deserialize<List<University>>(response);
            if (list != null)
                Universities = new ObservableCollection<University>(list);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hiba", $"Nem sikerült az egyetemeket betölteni: {ex.Message}", "OK");
        }
    }

    private void OnEditUser(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is User user)
        {
            SelectedUser = new User
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Note = user.Note,
                StartYear = user.StartYear,
                Status = user.Status,
                Active = user.Active,
                MajorId = user.MajorId
            };

            var uni = Universities.FirstOrDefault(u => u.Majors.Any(m => m.Id == int.Parse(user.MajorId)));
            if (uni != null)
            {
                SelectedUniversityId = uni.Id;
                AvailableMajors = new ObservableCollection<AppMajor>(uni.Majors);
            }

            BindingContext = null;
            BindingContext = this;
            EditPopup.IsVisible = true;
        }
    }

    private void OnUniversityChanged(object sender, EventArgs e)
    {
        if (SelectedUniversityId != null)
        {
            var uni = Universities.FirstOrDefault(u => u.Id == SelectedUniversityId);
            AvailableMajors = new ObservableCollection<AppMajor>(uni?.Majors ?? new List<AppMajor>());
            SelectedUser.MajorId = null;
            OnPropertyChanged(nameof(AvailableMajors));
        }
    }

    private async void OnSaveUser(object sender, EventArgs e)
    {
        try
        {
            using HttpClient client = new();
            string token = await SecureStorage.GetAsync("accessToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(JsonSerializer.Serialize(SelectedUser), Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{apiBaseUrl}/User/admin/{SelectedUser.Id}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Siker", "Felhasználó frissítve", "OK");
                EditPopup.IsVisible = false;
                LoadUsers();
            }
            else
            {
                await DisplayAlert("Hiba", "Nem sikerült frissíteni a felhasználót", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hiba", ex.Message, "OK");
        }
    }

    private async void OnDeleteUser(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is User user)
        {
            bool confirm = await DisplayAlert("Megerõsítés", "Biztosan törölni szeretnéd a felhasználót?", "Igen", "Nem");
            if (!confirm) return;

            try
            {
                using HttpClient client = new();
                string token = await SecureStorage.GetAsync("accessToken");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.DeleteAsync($"{apiBaseUrl}/User/admin/{user.Id}");
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Siker", "Felhasználó törölve", "OK");
                    LoadUsers();
                }
                else
                {
                    await DisplayAlert("Hiba", "Nem sikerült törölni", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hiba", ex.Message, "OK");
            }
        }
    }

    private void OnCancelEdit(object sender, EventArgs e)
    {
        EditPopup.IsVisible = false;
    }
}

public class ApiListResponse<T>
{
    public List<T> Values { get; set; } = new();
}
