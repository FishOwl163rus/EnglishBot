using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnglishBot.Interfaces;
using EnglishBot.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VkNet.Abstractions;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace EnglishBot
{
    public class BotService : BackgroundService
    {
        private readonly ILogger<BotService> _logger;
        private readonly IAsset _asset;
        private readonly IVkApi _api;
        private readonly IConfiguration _config;
        
        public BotService(ILogger<BotService> logger, IAsset asset, IVkApi api, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _asset = asset;
            _api = api;
            
            _asset.Load();
            _asset.CurrentIndex = int.Parse(config["CurrentIndex"]);
        }

        private void UploadAndPost(string path)
        {
            var group = long.Parse(_config["GroupID"]);
            
            var uploadServer = _api.Photo.GetWallUploadServer(group);
            var responseImg = Encoding.ASCII.GetString(new WebClient().UploadFile(uploadServer.UploadUrl, path)); 
            var savedContent = _api.Photo.SaveWallPhoto(responseImg, null, (ulong?) group).First();
            
            _api.Wall.Post(new WallPostParams
            {
                OwnerId = long.Parse(_config["AdminID"]),
                Attachments = new List<MediaAttachment> { savedContent }
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var (english, russian, transcription) = _asset.Words[_asset.CurrentIndex].GetTranslation(_config["YandexToken"]);
                
                _logger.Log(LogLevel.Information, "Новое слово: {0} -> {1} -> {2}.", russian, english, transcription);
                
                var newImage = ImageUtil.CreatePostImage(russian, english, transcription);
                
                _logger.Log(LogLevel.Information, "Создано изображение: {0}. Текуший индекс: {1}.", newImage, _asset.CurrentIndex);
                
                _asset.CurrentIndex++;
                
                UploadAndPost(newImage);

                await Task.Delay(1000 * 1800, stoppingToken);
            }
        }
    }
}