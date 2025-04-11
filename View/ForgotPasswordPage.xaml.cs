using System;
using Microsoft.Maui.Controls;
using Hephaistos_Maui.Models; 
using Hephaistos_Maui.Services; 

namespace Hephaistos_Maui.Pages
{
    public partial class ForgotPasswordPage : ContentPage
    {
        private string _email;
        private string _otp;
        private string _newPassword;
        private int _step = 1;

        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

     
        private async void OnSendOtpClicked(object sender, EventArgs e)
        {
            _email = EmailEntry.Text;
            if (string.IsNullOrWhiteSpace(_email))
            {
                await DisplayAlert("Hiba", "Kérlek add meg az email címed", "OK");
                return;
            }

            try
            {
                bool success = await AuthService.RequestOtpAsync(_email);
                if (success)
                {
                    _step = 2;
                    Step1.IsVisible = false;
                    Step2.IsVisible = true;
                }
                else
                {
                    await DisplayAlert("Hiba", "Sikertelen OTP kérés", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hiba", $"Hiba történt: {ex.Message}", "OK");
            }
        }

        
        private async void OnResetPasswordClicked(object sender, EventArgs e)
        {
            _otp = OtpEntry.Text;
            _newPassword = NewPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(_otp) || string.IsNullOrWhiteSpace(_newPassword))
            {
                await DisplayAlert("Hiba", "Minden mezõt ki kell tölteni", "OK");
                return;
            }

            try
            {
                bool success = await AuthService.VerifyOtpAsync(_email, _otp, _newPassword);
                if (success)
                {
                    await DisplayAlert("Siker", "A jelszó sikeresen módosítva", "OK");
                   
                }
                else
                {
                    await DisplayAlert("Hiba", "Sikertelen jelszó módosítás", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hiba", $"Hiba történt: {ex.Message}", "OK");
            }
        }
    }
}
