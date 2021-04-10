using System;
using System.IO;
using System.Numerics;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace EnglishBot.Utils
{
    public static class ImageUtil
    {
        private static readonly FontCollection Collection = new FontCollection();
        private static readonly Font FontEng = InstallFont("wwwroot/assets/fonts/FiraSans-Medium.ttf", 140);
        private static readonly Font FontRus = InstallFont("wwwroot/assets/fonts/FiraSans-Regular.ttf", 110);
        private static readonly Font FontTranscription = InstallFont("wwwroot/assets/fonts/FiraSans-Regular.ttf", 70);

        private static Vector2 GetCenter(this IImageInfo image) => new Vector2(0,  (float) image.Height/2);
        private static Font InstallFont(string path, byte size)
        {
            var family = Collection.Install(path);
            return family.CreateFont(size, FontStyle.Regular);
        }
        
        public static string CreatePostImage(string rus, string eng, string transcription)
        {
            using var image = Image.Load(File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "wwwroot/assets/template.jpg")));
            
            var center = image.GetCenter();
            var wrap = image.Width;
                
            image.Mutate(i =>
            {
                i.DrawText(new TextGraphicsOptions(new GraphicsOptions(), new TextOptions
                {
                    WrapTextWidth = wrap,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                }), eng, FontEng, Color.White, new Vector2(center.X, center.Y - 60));
                    
                i.DrawText(new TextGraphicsOptions(new GraphicsOptions(), new TextOptions
                {
                    WrapTextWidth = wrap,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }), rus, FontRus, Color.White, new Vector2(center.X, center.Y + 100));
                    
                i.DrawText(new TextGraphicsOptions(new GraphicsOptions(), new TextOptions
                {
                    WrapTextWidth = wrap,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center
                }), $"[{transcription}]", FontTranscription, Color.White, new Vector2(-30, 50));
            });
                
            var outputPath = $"wwwroot/tmp_images/word_{eng}.jpg";
            image.Save(outputPath);

            return outputPath;
        }
    }
}