namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.IO;
    using System.Windows.Media.Imaging;

    public class WebSequenceDiagramsResult
    {
        public Byte[] ImageData { get; private set; }

        public Int32 ImageWidth { get; private set; }
        public Int32 ImageHeight { get; private set; }

        public WebSequenceDiagramsResult(Byte[] imageData)
        {
            this.ImageData = imageData;
        }

        public BitmapImage GetBitmapImage()
        {
            var bitmap = new BitmapImage();
            using (var stream = new MemoryStream(this.ImageData))
            {
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
            }

            return bitmap;
        }
    }
}
