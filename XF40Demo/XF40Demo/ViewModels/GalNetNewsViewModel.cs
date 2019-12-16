using Acr.UserDialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF40Demo.Convertors;
using XF40Demo.Helpers;
using XF40Demo.Models;
using XF40Demo.Services;

namespace XF40Demo.ViewModels
{
    public class GalNetNewsViewModel : BaseViewModel
    {
        #region Properties

        public ICommand RetryDownloadCommand { get; }

        public ObservableCollection<NewsItem> GalNetNewsList { get; set; }

        private DateTime _lastUpdated;
        public DateTime LastUpdated
        {
            get { return _lastUpdated; }
            private set
            {
                if (_lastUpdated != value)
                {
                    _lastUpdated = value;
                    OnPropertyChanged(nameof(LastUpdated));
                }
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }

        #endregion

        public GalNetNewsViewModel()
        {
            RetryDownloadCommand = new Command(() => RetryGalNetNewsAsync());
            GalNetNewsList = new ObservableCollection<NewsItem>();
        }

        private async void GetGalNetNewsAsync(CancellationTokenSource cancelToken, bool ignoreCache = false)
        {
            using (UserDialogs.Instance.Loading("Loading", () => cancelToken.Cancel(), null, true, MaskType.Clear))
            {
                try
                {
                    // get the news feed
                    string json = String.Empty;
                    GalNetService news = new GalNetService();
                    (json, LastUpdated) = await news.GetData(cancelToken, ignoreCache).ConfigureAwait(false);

                    // parse the json data
                    GalNetNewsList.Clear();
                    await Task.Run(() =>
                    {
                        List<NewsItem> fullNews = JsonConvert.DeserializeObject<List<NewsItem>>(json, NewsItemConverter.Instance);
                        foreach (NewsItem item in fullNews.Where(o => !String.IsNullOrEmpty(o.Body)).OrderByDescending(o => o.PublishDateTime).Take(20))
                        {
                            item.ClassifyArticle();
                            Device.BeginInvokeOnMainThread(() => GalNetNewsList.Add(item));
                        }
                    }).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    SetMessage("GalNet News download was cancelled or timed out.", true);
                }
                catch (HttpRequestException ex)
                {
                    string err = ex.Message;
                    int start = err.IndexOf("OPENSSL_internal:", StringComparison.OrdinalIgnoreCase);
                    if (start > 0)
                    {
                        start += 17;
                        int end = err.IndexOf(" ", start, StringComparison.OrdinalIgnoreCase);
                        err = String.Format("SSL Error ({0})", err.Substring(start, end - start).Trim());
                    }
                    else if (err.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        err = err.Substring(err.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) + 6).Trim();
                    }
                    SetMessage(String.Format("Network Error: {0}", err), true);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("unexpected end of stream"))
                    {
                        SetMessage("GalNet News download was cancelled.", true);
                    }
                    else
                    {
                        SetMessage(String.Format("Error: {0}", ex.Message), true);
                    }
                }
            }
        }

        private void SetMessage(string message, Boolean isError)
        {
            Message = message;
            if (isError)
            {
                ToastHelper.Toast(message);
            }
        }

        private async void RetryGalNetNewsAsync()
        {
            CancellationTokenSource cancelToken = new CancellationTokenSource();
            await Task.Run(() => GetGalNetNewsAsync(cancelToken, true)).ConfigureAwait(false);
        }

        protected override async void RefreshView()
        {
            CancellationTokenSource cancelToken = new CancellationTokenSource();
            await Task.Run(() => GetGalNetNewsAsync(cancelToken)).ConfigureAwait(false);
        }
    }
}
