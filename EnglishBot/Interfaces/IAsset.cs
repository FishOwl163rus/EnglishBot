using System.Collections.Generic;

namespace EnglishBot.Interfaces
{
    public interface IAsset
    {
        int CurrentIndex { get; set; }
        void Load();
        void Reset();
        List<string> Words { get; set; }
    }
}