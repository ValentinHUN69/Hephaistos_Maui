using System.Collections.ObjectModel;
using System.Text.Json;
using Hephaistos_Maui.Models;
using System.Net.Http;

namespace Hephaistos_Maui.View;

public partial class SchedulePage : ContentPage
{
    public ObservableCollection<DaySchedule> WeeklySchedule { get; set; } = new();
    public bool IsLoading { get; set; } = false;

    public SchedulePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnGenerateClicked(object sender, EventArgs e)
    {
        IsLoading = true;
        OnPropertyChanged(nameof(IsLoading));

        try
        {
            using HttpClient client = new();
            var requestContent = new StringContent("20", System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://hephaistos-backend-c6c5ewhraedvgzex.germanywestcentral-01.azurewebsites.net/api/TimetableGenerator/generate", requestContent);
            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Hiba", "Nem sikerült lekérni az órarendet", "OK");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TimetableResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result?.Timetable?.Values != null)
            {
                var grouped = result.Timetable.Values
                    .GroupBy(cls => cls.DayOfWeek)
                    .OrderBy(g => g.Key)
                    .Select(g => new DaySchedule
                    {
                        Day = g.Key,
                        Classes = new ObservableCollection<ClassItem>(g.OrderBy(c => c.StartTime))
                    });

                WeeklySchedule.Clear();
                foreach (var group in grouped)
                    WeeklySchedule.Add(group);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hiba történt: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
            OnPropertyChanged(nameof(IsLoading));
        }
    }
}
