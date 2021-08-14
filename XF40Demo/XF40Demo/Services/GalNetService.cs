using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using XF40Demo.Convertors;
using XF40Demo.Models;

namespace XF40Demo.Services
{
    public sealed class GalNetService
    {
        private static readonly GalNetService instance = new GalNetService();

        private static DownloadService dService;

        private const string GalNetURL = "https://api.taranissoftware.com/elite-dangerous/galnet-latest.json";

        private readonly List<NewsArticle> galnetNews;
        private DateTime lastUpdated;

        private GalNetService()
        {
            lastUpdated = DateTime.MinValue;
            galnetNews = new List<NewsArticle>();
        }

        public static GalNetService Instance()
        {
            dService = DownloadService.Instance();
            return instance;
        }

        public async Task<(List<NewsArticle> news, DateTime updated)> GetData(int articleCount,
                                                                              int expiryHours,
                                                                              CancellationTokenSource cancelToken,
                                                                              string BoW = null,
                                                                              string ignoreBoW = null,
                                                                              bool ignoreCache = false)
        {
            if (expiryHours < 1) expiryHours = 1;

            TimeSpan expiry = TimeSpan.FromHours(expiryHours);
            if (galnetNews?.Any() == false || galnetNews.Count < articleCount || lastUpdated + expiry < DateTime.Now)
            {
                // download the json
                string json;
                DownloadOptions options = new DownloadOptions(cancelToken, expiry, ignoreCache);
                (json, lastUpdated) = await dService.GetData(GalNetURL, options).ConfigureAwait(false);

                // parse the news articles
                galnetNews.Clear();
                await Task.Run(() =>
                {
                    List<NewsArticle> fullNews = JsonConvert.DeserializeObject<List<NewsArticle>>(json, NewsArticleConverter.Instance());
                    List<Topic> topics = GetTopics(BoW);
                    List<Topic> ignoreTopics = GetIgnoreTopics(ignoreBoW);
                    foreach (NewsArticle item in fullNews.Where(o => !string.IsNullOrEmpty(o.Body)).OrderByDescending(o => o.PublishDateTime).Take(articleCount))
                    {
                        item.ClassifyArticle(topics, ignoreTopics);
                        galnetNews.Add(item);

                        foreach (Topic topic in topics)
                            topic.Count = 0;
                        foreach (Topic topic in ignoreTopics)
                            topic.Count = 0;
                    }
                }).ConfigureAwait(false);
            }
            return (galnetNews.Take(articleCount).ToList(), lastUpdated);
        }

        private List<Topic> GetTopics(string BoW)
        {
            return string.IsNullOrWhiteSpace(BoW)
                   ? LoadBoW("XF40Demo.Resources.NewsBoW.json")
                   : JsonConvert.DeserializeObject<List<Topic>>(BoW);
        }

        private List<Topic> GetIgnoreTopics(string ignoreBoW)
        {
            return string.IsNullOrWhiteSpace(ignoreBoW)
                   ? LoadBoW("XF40Demo.Resources.NewsFalseBoW.json")
                   : JsonConvert.DeserializeObject<List<Topic>>(ignoreBoW);
        }

        private List<Topic> LoadBoW(string filename)
        {
            List<Topic> topics;
            try
            {
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream(filename))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        topics = (List<Topic>)serializer.Deserialize(reader, typeof(List<Topic>));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to load Bag of Words for linguistic analysis.", ex);
            }
            return topics;
        }
    }
}
