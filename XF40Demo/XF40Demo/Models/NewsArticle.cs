using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XF40Demo.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class NewsArticle
    {
        #region Properties

        [JsonProperty(PropertyName = "uid")]
        public string Id { get; set; }

        private string _title;
        [JsonProperty(PropertyName = "title")]
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value.Trim();
                }
            }
        }

        private string _body;
        [JsonProperty(PropertyName = "body")]
        public string Body
        {
            get { return _body; }
            set
            {
                if (_body != value)
                {
                    _body = value.Replace("<p>", "").Replace("</p>", "\n").Replace("<br />", "\n").Replace("&#039;", "'").Replace("&quot;", "\u201d").Trim();
                }
            }
        }

        [JsonProperty(PropertyName = "date")]
        public DateTime PublishDateTime { get; set; }

        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        public string Topic { get; private set; }

        public List<string> Tags { get; private set; }

        public string PublishDate
        {
            get
            {
                string date = PublishDateTime.ToString(new CultureInfo("en-GB"));
                return date.Substring(0, date.IndexOf(" "));
            }
        }

        #endregion

        protected internal void ClassifyArticle(List<Topic> topics, List<Topic> ignoreTopics)
        {
            Tags = new List<string>();
            List<string> sentences = SplitSentences();

            // analyse article using Bag of Words technique
            AnalyseSentences(sentences, topics);

            // analyse article again to identify false positives
            AnalyseSentences(sentences, ignoreTopics);

            // subtract false positives from topics list
            foreach (Topic falseTopic in ignoreTopics)
            {
                Topic topic = topics.Find(x => x.Name.Equals(falseTopic.Name));
                topic.Count -= falseTopic.Count;
            }

            // select topic + tags
            Topic tempTopic = topics.OrderByDescending(o => o.Count).First();
            Topic = (tempTopic.Count < 2 || string.Equals(Title, "week in review", StringComparison.OrdinalIgnoreCase)) ? "Unclassified" : tempTopic.Name;
            foreach (Topic topic in topics.OrderByDescending(o => o.Count).Take(4))
            {
                if (topic.Count > 0)
                {
                    Tags.Add(topic.Name);
                }
            }
        }

        private void AnalyseSentences(List<string> sentences, List<Topic> topicsList)
        {
            foreach (string sentence in sentences)
            {
                Parallel.ForEach(topicsList, topic =>
                {
                    foreach (string term in topic.Terms)
                    {
                        if (sentence.Contains(term.ToLower()))
                        {
                            topic.Count++;
                        }
                    }
                });
            }
        }

        private List<string> SplitSentences()
        {
            List<string> sentences = new List<string>
            {
                Title.Trim().ToLower(),
                Title.Trim().ToLower()
            };
            foreach (string sentence in Regex.Split(Body, @"(?<=[\w\s](?:[\.\!\? ]+[\x20]*[\x22\xBB]*))(?:\s+(?![\x22\xBB](?!\w)))"))
            {
                sentences.Add(sentence.Trim().ToLower());
            }
            return sentences;
        }

        public override string ToString()
        {
            return $"{Title}: {Body}";
        }
    }
}
