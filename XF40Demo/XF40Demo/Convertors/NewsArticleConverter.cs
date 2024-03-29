﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using XF40Demo.Models;

namespace XF40Demo.Convertors
{
    internal sealed class NewsArticleConverter : JsonConverter
    {
        private static readonly NewsArticleConverter instance = new NewsArticleConverter();

        private static readonly Type NewsArticleType = typeof(ICollection<NewsArticle>);

        private NewsArticleConverter() { }

        public static NewsArticleConverter Instance()
        {
            return instance;
        }

        public override bool CanConvert(Type objectType)
        {
            return NewsArticleType.IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var collection = new List<NewsArticle>();
            NewsArticle item = null;
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        item = new NewsArticle();
                        collection.Add(item);
                        break;
                    case JsonToken.PropertyName:
                        SetProperty(reader, item);
                        break;
                    case JsonToken.EndArray:
                        return collection;
                }
            }
            return collection;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private static void SetProperty(JsonReader reader, NewsArticle item)
        {
            var name = (string)reader.Value;
            reader.Read();
            switch (name)
            {
                case "uid":
                    item.Id = (string)reader.Value;
                    break;
                case "title":
                    item.Title = (string)reader.Value;
                    break;
                case "body":
                    item.Body = (string)reader.Value;
                    break;
                case "date":
                    item.PublishDateTime = Convert.ToDateTime(reader.Value, new CultureInfo("en-GB"));
                    break;
                case "link":
                    item.Link = (string)reader.Value;
                    break;
            }
        }
    }
}
