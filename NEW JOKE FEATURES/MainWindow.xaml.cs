using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JokeFetcherWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void FetchJoke_Click(object sender, RoutedEventArgs e)
        {
            JokeText.Text = "জোক লোড হচ্ছে...⏳";
            try
            {
                string category = ((ComboBoxItem)CategoryBox.SelectedItem).Content.ToString();
                string joke = await GetJokeAsync(category);
                JokeText.Text = TranslateToBangla(joke);
            }
            catch (Exception ex)
            {
                JokeText.Text = $"❌ সমস্যা হয়েছে: {ex.Message}";
            }
        }

        private async void FetchMultipleJokes_Click(object sender, RoutedEventArgs e)
        {
            JokeText.Text = "জোক লোড হচ্ছে...⏳";
            try
            {
                string category = ((ComboBoxItem)CategoryBox.SelectedItem).Content.ToString();
                List<string> jokes = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    string joke = await GetJokeAsync(category);
                    jokes.Add(TranslateToBangla(joke));
                }
                JokeText.Text = string.Join("\n\n", jokes);
            }
            catch (Exception ex)
            {
                JokeText.Text = $"❌ সমস্যা হয়েছে: {ex.Message}";
            }
        }

        private async Task<string> GetJokeAsync(string category)
        {
            using HttpClient client = new HttpClient();
            string url = $"https://official-joke-api.appspot.com/jokes/{category}/random";

            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            dynamic jokeObj = JsonConvert.DeserializeObject(json.StartsWith("[") ? json.Trim('[', ']') : json);

            return $"{jokeObj.setup} - {jokeObj.punchline}";
        }

        private void CopyJoke_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(JokeText.Text);
            MessageBox.Show("জোক কপি হয়েছে!");
        }

        private string TranslateToBangla(string text)
        {
            // Basic manual translation (for demo)
            return text.Replace("Why", "কেন")
                       .Replace("did", "করেছিল")
                       .Replace("Because", "কারণ")
                       .Replace("What", "কি")
                       .Replace("How", "কিভাবে");
        }
    }
}