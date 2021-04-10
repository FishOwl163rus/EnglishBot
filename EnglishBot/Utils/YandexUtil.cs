using System;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace EnglishBot.Utils
{
    public static class YandexUtil
    {
        public static (string, string, string) GetTranslation(this string word, string apiKey)
        {
            var url = $"https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key={apiKey}&lang=en-ru&text={word}";
            var client = new WebClient();
            var content =  Encoding.UTF8.GetString(client.DownloadData(url));
            
            var arrObj = JObject.Parse(content);

            var eng = (string) arrObj["def"]?[0]?["text"];
            var rus = (string) arrObj["def"]?[0]?["tr"]?[0]?["text"];
            var transcription = (string) arrObj["def"]?[0]?["ts"];
            
            return (eng, rus, transcription);
        }
    }
    
}