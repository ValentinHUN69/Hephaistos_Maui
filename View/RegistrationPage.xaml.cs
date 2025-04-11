using System.Text.Json;
using Hephaistos_Maui.View;

using System.Text;

namespace Hephaistos_Maui.View;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage()
	{
		InitializeComponent();
	}

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text?.Trim();
        string email = EmailEntry.Text?.Trim();
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            await DisplayAlert("Hiba", "Minden mezõt ki kell tölteni!", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Hiba", "A jelszavak nem egyeznek!", "OK");
            return;
        }

        bool isRegistered = await MauiProgram.RegisterAsync(username, email, password);


        if (isRegistered)
        {
            await DisplayAlert("Siker", "Sikeresen regisztráltál!", "OK");
            await Navigation.PushAsync(new LoginPage());
        }
        else
        {
            await DisplayAlert("Hiba", "Hiba történt a regisztráció során!", "OK");
        }
    }
}