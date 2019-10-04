using FilesExercise.Domain.Constants;
using FilesExercise.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FilesExercise
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            try
            {
                string userSettingsPath = GetUserSettingsPath();

                if (File.Exists(userSettingsPath))
                {
                    UserSettings userSettings = JsonConvert.DeserializeObject<UserSettings>(File.ReadAllText(userSettingsPath));

                    entUserName.Text = userSettings.UserName;
                    entAge.Text = userSettings.Age.ToString();
                    entEmail.Text = userSettings.Email.ToString();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            base.OnAppearing();
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                UserSettings userSettings = new UserSettings
                {
                    UserName = entUserName.Text,
                    Age = int.Parse(entAge.Text),
                    Email = entEmail.Text
                };

                string serializedSettings = JsonConvert.SerializeObject(userSettings);
                File.WriteAllText(GetUserSettingsPath(), serializedSettings);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private string GetUserSettingsPath() 
        {
            string appFolder = FileSystem.AppDataDirectory;
            string userSettingsPath = Path.Combine(appFolder, AppSaveFile.Settings);

            return userSettingsPath;
        }
    }
}