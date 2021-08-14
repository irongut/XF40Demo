using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF40Demo.Helpers;
using XF40Demo.Models;
using XF40Demo.Services;

namespace XF40Demo.ViewModels
{
    public class GalNetNewsViewModel : BaseViewModel
    {
        #region Properties

        public ICommand RetryDownloadCommand { get; }

        public ObservableCollection<NewsArticle> GalNetNewsList { get; set; }

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
            GalNetNewsList = new ObservableCollection<NewsArticle>();
        }

        private async void GetGalNetNewsAsync(CancellationTokenSource cancelToken, bool ignoreCache = false)
        {
            if (GalNetNewsList?.Any() == false)
            {
                using (UserDialogs.Instance.Loading("Loading", () => cancelToken.Cancel(), null, true, MaskType.Clear))
                {
                    try
                    {
                        List<NewsArticle> newsList = new List<NewsArticle>();
                        GalNetService news = GalNetService.Instance();
                        (newsList, LastUpdated) = await news.GetData(12, settings.NewsCacheTime, cancelToken, ignoreCache: ignoreCache).ConfigureAwait(false);

                        GalNetNewsList.Clear();
                        foreach (NewsArticle item in newsList)
                        {
                            GalNetNewsList.Add(item);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        SetMessage("GalNet News download was cancelled or timed out.", true);
                    }
                    catch (HttpRequestException ex)
                    {
                        string errorMessage = ex.Message;
                        if (errorMessage.IndexOf("OPENSSL_internal:", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            errorMessage = "A secure connection could not be established.";
                        }
                        else if (errorMessage.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            errorMessage = errorMessage.Substring(errorMessage.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) + 6).Trim();
                        }
                        SetMessage($"Network Error: {errorMessage}", true);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("unexpected end of stream"))
                        {
                            SetMessage("GalNet News download was cancelled.", true);
                        }
                        else
                        {
                            SetMessage($"Error: {ex.Message}", true);
                        }
                    }
                }
            }
        }

        private void SetMessage(string message, bool isError)
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
