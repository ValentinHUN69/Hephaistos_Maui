using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using Hephaistos_Maui.Models;

namespace Hephaistos_Maui.View
{
    public partial class Subjects : ContentPage
    {
        static string connection = "https://hephaistos-backend-c6c5ewhraedvgzex.germanywestcentral-01.azurewebsites.net/api";
        public ObservableCollection<Subject> Subjectss { get; set; } = new();

        public Subjects()
        {
            InitializeComponent();
            SubjectsListView.ItemsSource = Subjectss;
            LoadSubjects();
        }

        private async void LoadSubjects()
        {
            var subjects = await MauiProgram.GetSubjectsAsync();

            if (subjects == null)
            {
                await DisplayAlert("Hiba", "Nem sikerült betölteni a tantárgyakat.", "OK");
                return;
            }

            Subjectss.Clear();
            foreach (var subject in subjects)
            {
                Subjectss.Add(subject);
            }
        }

        private async void OnSubjectSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Subject selectedSubject)
            {
                await DisplayAlert("Tantárgy", $"Név: {selectedSubject.Name}\nKód: {selectedSubject.Code}", "OK");
                ((ListView)sender).SelectedItem = null; 
            }
        }
    }

    
}
