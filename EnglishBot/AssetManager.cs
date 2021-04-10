using System.Collections.Generic;
using System.IO;
using EnglishBot.Interfaces;
using EnglishBot.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EnglishBot
{
    public class AssetManager : IAsset
    {
        private int _currentIndex;

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                _currentIndex = value;
                ConfigUtil.AddOrUpdateAppSetting("CurrentIndex", _currentIndex);
            }
        }

        public void Load()
        {
            if (!File.Exists("wwwroot/assets/english.txt")) throw new FileNotFoundException("Filed to find asset file!");
            
            Words = new List<string>(File.ReadAllLines("wwwroot/assets/english.txt"));
        }

        public void Reset() => CurrentIndex = 0;
        
        public List<string> Words { get; set; }
    }
}