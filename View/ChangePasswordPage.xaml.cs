using Hephaistos_Maui.Models;
using Microsoft.Maui.Controls;

namespace Hephaistos_Maui.Pages
{
    public partial class ChangePasswordPage : ContentPage
    {
        private ChangePasswordViewModel _viewModel;

        public ChangePasswordPage()
        {
            InitializeComponent();
            _viewModel = new ChangePasswordViewModel();
            BindingContext = _viewModel;
        }

        
        private void OnChangePasswordClicked(object sender, EventArgs e)
        {
           
            _viewModel.ChangePasswordCommand.Execute(null);
        }
    }
}
