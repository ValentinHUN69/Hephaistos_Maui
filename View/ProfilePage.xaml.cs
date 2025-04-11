using System.Collections.ObjectModel;
using System.Text.Json;
using Hephaistos_Maui.Models;


namespace Hephaistos_Maui.View;

public partial class ProfilePage : ContentPage
{
    public ObservableCollection<Subject> CompletedSubjects { get; set; } = new();
    public ObservableCollection<Subject> AvailableSubjects { get; set; } = new();

    public User EditableUser { get; set; } = new();
    public bool IsEditing { get; set; } = false;

    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = this;
        LoadProfile();
        LoadSubjects();
    }

    private async void LoadProfile()
    {
        var user = await MauiProgram.LoadUserAsync();
        if (user != null)
        {
            EditableUser = user;
            OnPropertyChanged(nameof(EditableUser));
        }
    }

    private async void LoadSubjects()
    {
        var subjects = await MauiProgram.LoadSubjectsAsync();
        if (subjects != null)
        {
            AvailableSubjects.Clear();
            foreach (var subject in subjects)
            {
                AvailableSubjects.Add(subject);
            }
        }
    }

    private async void SaveProfile(object sender, EventArgs e)
    {
        var success = await MauiProgram.SaveUserProfileAsync(EditableUser);
        if (success)
        {
            await DisplayAlert("Siker", "Profil mentve", "OK");
            IsEditing = false;
            OnPropertyChanged(nameof(IsEditing));
        }
        else
        {
            await DisplayAlert("Hiba", "Nem sikerült menteni a profilt", "OK");
        }
    }

    private void EnableEditing(object sender, EventArgs e)
    {
        IsEditing = true;
        OnPropertyChanged(nameof(IsEditing));
    }

    private void CancelEditing(object sender, EventArgs e)
    {
        IsEditing = false;
        LoadProfile();
    }

    private async void DeactivateProfile(object sender, EventArgs e)
    {
        var confirm = await DisplayAlert("Megerõsítés", "Biztosan inaktiválni szeretnéd a profilodat?", "Igen", "Nem");
        if (confirm)
        {
            var success = await MauiProgram.DeactivateProfileAsync();
            if (success)
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                await DisplayAlert("Hiba", "Nem sikerült inaktiválni a profilt", "OK");
            }
        }
    }

    private void ToggleSubject(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = sender as CheckBox;
        var subject = checkbox?.BindingContext as Subject;
        if (subject != null)
        {
            if (e.Value && !CompletedSubjects.Contains(subject))
                CompletedSubjects.Add(subject);
            else if (!e.Value && CompletedSubjects.Contains(subject))
                CompletedSubjects.Remove(subject);
        }
    }

    private async void SaveSubjects(object sender, EventArgs e)
    {
        var success = await MauiProgram.SaveSubjectsAsync(CompletedSubjects.ToList());
        if (success)
        {
            await DisplayAlert("Siker", "Tárgyak mentve", "OK");
        }
        else
        {
            await DisplayAlert("Hiba", "Nem sikerült menteni a tárgyakat", "OK");
        }
    }
}
