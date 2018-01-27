using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebhoseApiTest.Extensions;
using WebhoseApiTest.Models;

namespace WebhoseApiTest
{
    public class MainPageViewModel : BindableBase, IDisposable
    {
        public static MainPageViewModel _myMainPageViewModel = null;

        public static MainPageViewModel Instance
        {
            get => _myMainPageViewModel ?? GetInstance();
            set => _myMainPageViewModel = value;
        }

        private static MainPageViewModel GetInstance()
        {
            return _myMainPageViewModel ?? (_myMainPageViewModel = new MainPageViewModel());
        }

        public MainPageViewModel()
        {
            DisplayText = "This is the main page!";
            InitializeCommands();
        }

        public string DisplayText { get; set; }

        public WebhoseResponce CurrentResponce { get; set; }

        public ObservableCollection<WebhosePost> CurrentPosts { get; set; } = new ObservableCollection<WebhosePost>();

        private WebhosePost _mySelectedPost = null;
        public WebhosePost SelectedPost
        {
            get => _mySelectedPost;
            set => SetProperty(ref _mySelectedPost, value);
        }

        public DelegateCommand GetNewsFromWebHoseCommand { get; set; }

        public DelegateCommand PostToSlackSelectedResponceCommand { get; set; }

        public DelegateCommand SpeakSelectedPostCommand { get; set; }

        public string UrlText { get; set; } =
            "http://webhose.io/filterWebContent?token=58bf37e3-5930-40fb-8ee8-1c5ca178ce70&format=json&ts=1512963018988&sort=crawled&q=(title%3A%22CSharp%22%20OR%20title%3A%22Rx.io%22%20OR%20title%3A%22kotlin%22%20OR%20title%3A%22xamarin%22%20OR%20title%3A%22RxJS%22%20OR%20title%3A%22RxKotlin%22%20OR%20title%3A%22GoLang%22%20OR%20title%3A%22RxGo%22)%20language%3Ajapanese%20thread.country%3AJP";

        private HttpClient _myWebhoseClient = null;

        private HttpClient _mySlackHookClient = null;

        private void InitializeCommands()
        {
            GetNewsFromWebHoseCommand = new DelegateCommand(GetNewsFromWebHose);
            PostToSlackSelectedResponceCommand = new DelegateCommand(PostToSlackSelected);
            SpeakSelectedPostCommand = new DelegateCommand(SpeakSelectedPost);
        }

        private void SpeakSelectedPost()
        {
            SelectedPost?.text.Speak();
        }

        private async void GetNewsFromWebHose()
        {
            var message = "";
            var header = "";
            CurrentResponce = null;
            CurrentPosts.Clear();

            try
            {
                if (_myWebhoseClient == null) _myWebhoseClient = new HttpClient();

                var res = await _myWebhoseClient.GetStringAsync(UrlText);

                DisplayText = res;
                CurrentResponce = JsonConvert.DeserializeObject<WebhoseResponce>(res);

                if (CurrentResponce.posts.Any())
                {
                    SelectedPost = CurrentResponce.posts.Random();
                    CurrentPosts.AddRange(CurrentResponce.posts);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void PostToSlackSelected()
        {
            await PostToSlask(SelectedPost);
        }

        private async Task PostToSlask(WebhosePost post)
        {
            if (post == null) return;

            var header = post?.thread?.title;
            var message = post?.thread?.url;

            var webhookUrl = "https://hooks.slack.com/services/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";
            var payload = new
            {
                channel = "#demo",
                username = "開発課Bot デブ君",
                text = $"おすすめの記事です。 \r\n ●{header}\r\n\r\n{message}",
                icon_emoji = ":baby:",
                mrkdwn = true,
            };

            var jsonString = JsonConvert.SerializeObject(payload);

            DisplayText = jsonString;

            if (_mySlackHookClient == null) _mySlackHookClient = new HttpClient();

            var res = await _mySlackHookClient.PostAsync(webhookUrl,
                new StringContent(jsonString, Encoding.UTF8, "application/json"));

            //todo:resを使って何か一言
        }

        public void Dispose()
        {
            if (_myWebhoseClient != null) _myWebhoseClient.Dispose();
            if (_mySlackHookClient != null) _mySlackHookClient.Dispose();
        }
    }
}
