using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Threading.Tasks;
using Hephaistos_Maui.Models;  // Importáljuk a ChangePasswordModel osztályt

namespace Hephaistos_Maui.Models
{
    public class ChangePasswordViewModel : INotifyPropertyChanged
    {
        private string _currentPassword;
        private string _newPassword;
        private string _confirmNewPassword;

        public string CurrentPassword
        {
            get => _currentPassword;
            set
            {
                if (_currentPassword != value)
                {
                    _currentPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                if (_newPassword != value)
                {
                    _newPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ConfirmNewPassword
        {
            get => _confirmNewPassword;
            set
            {
                if (_confirmNewPassword != value)
                {
                    _confirmNewPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ChangePasswordCommand { get; }

        public ChangePasswordViewModel()
        {
            ChangePasswordCommand = new Command(OnChangePassword);
        }

        private async void OnChangePassword()
        {
            // Ellenőrizzük, hogy minden mező ki van-e töltve
            if (string.IsNullOrEmpty(CurrentPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmNewPassword))
            {
                // Itt kezelheted a hibát, pl. egy üzenetet jeleníthetsz meg
                return;
            }

            // Ellenőrizzük, hogy az új jelszó és a megerősítő jelszó megegyeznek-e
            if (NewPassword != ConfirmNewPassword)
            {
                // Itt is kezelheted a hibát, pl. értesítheted a felhasználót
                return;
            }

            // Ellenőrizzük, hogy az új jelszó elég erős-e (pl. minimum 8 karakter)
            if (NewPassword.Length < 8)
            {
                // További hibaüzenet megjelenítése, ha a jelszó túl rövid
                return;
            }

            try
            {
               
                var changePasswordModel = new ChangePasswordModel
                {
                    OldPassword = CurrentPassword,
                    NewPassword = NewPassword,
                    ConfirmPassword = ConfirmNewPassword
                };

                bool success = await ChangePasswordApi.ChangePasswordAsync(changePasswordModel);

                if (success)
                {
                    
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
