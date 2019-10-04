using FilesExercise.Domain.Constants;
using FilesExercise.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FilesExercise
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private string computerChoice;
        private readonly string[] Choices = new string[] { "Rock", "Paper", "Scissors" };
        private static Random Random;
        private GameSettings gameSettings;


        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            string gameStatsPath = GetGameStatsPath();
            if (File.Exists((gameStatsPath)))
            {
                using(var reader = new StreamReader(gameStatsPath))
                {
                    string serializedGameStats = reader.ReadToEnd();
                    gameSettings = JsonConvert.DeserializeObject<GameSettings>(serializedGameStats);
                    UpdateGameStats();
                }
            }

            else
            {
                gameSettings = new GameSettings();
                UpdateGameStats();
            }
            
            
            computerChoice = MakeComputerChoice();
            base.OnAppearing();
        }

        private async void Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        private void RockPaperScissors_Clicked(object sender, EventArgs e)
        {
            string userChoice = (string)((Button)sender).CommandParameter;
            DetermineWinner(userChoice);
            
            //reset and save
            computerChoice = MakeComputerChoice();
            string serializedGameSettings = JsonConvert.SerializeObject(gameSettings);
            File.WriteAllText(GetGameStatsPath(), serializedGameSettings);
        }

        private string MakeComputerChoice()
        {
            Random = new Random();
            int choice = Random.Next(0, Choices.Length);
            return Choices[choice];
        }

        private void DetermineWinner(string userChoice)
        {
            if(userChoice == computerChoice)
            {
                Draw();
            }
            else
            {
                switch(userChoice)
                {
                    case "Rock":
                        if (computerChoice == "Paper") Lose();
                        else Win();
                        break;

                    case "Paper":
                        if (computerChoice == "Scissors") Lose();
                        else Win();
                        break;

                    case "Scissors":
                        if (computerChoice == "Rock") Lose();
                        else Win();
                        break;
                }
            }
        }

        private void UpdateGameStats()
        {
            lblLosses.Text = gameSettings.Losses.ToString();
            lblWins.Text = gameSettings.Wins.ToString();
            lblDraws.Text = gameSettings.Draws.ToString();
        }

        private void Lose()
        {
            frWinOrLose.BackgroundColor = Color.IndianRed;

            gameSettings.Losses++;
            lblLosses.Text = gameSettings.Losses.ToString();
            lblComputerChoice.Text = ComputerChoiceMessage(computerChoice);
            lblWinOrLose.Text = "You Lose!";
        }

        private void Win()
        {
            frWinOrLose.BackgroundColor = Color.LightGreen;
            
            gameSettings.Wins++;
            lblWins.Text = gameSettings.Wins.ToString();
            lblComputerChoice.Text = ComputerChoiceMessage(computerChoice);
            lblWinOrLose.Text = "You Win!";
        }

        private void Draw()
        {
            frWinOrLose.BackgroundColor = Color.Orange;

            gameSettings.Draws++;
            lblDraws.Text = gameSettings.Draws.ToString();
            lblComputerChoice.Text = ComputerChoiceMessage(computerChoice);
            lblWinOrLose.Text = "Draw!";
        }

        private string ComputerChoiceMessage(string result)
        {
            return $"Computer chose {result.ToLower()}";
        }

        private string GetGameStatsPath()
        {
            string applicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string gameStatsPath = Path.Combine(applicationDataFolder, AppSaveFile.GameStats);

            return gameStatsPath;
        }
    }
}
