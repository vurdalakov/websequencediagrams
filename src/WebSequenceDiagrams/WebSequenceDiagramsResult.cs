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

        public void SaveImage(String fileName, Int32 format)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                using (var memoryStream = new MemoryStream(this.ImageData))
                {
                    BitmapEncoder encoder;
                    switch (format)
                    {
                        case 1:
                            encoder = new BmpBitmapEncoder();
                            break;
                        case 2:
                            encoder = new GifBitmapEncoder();
                            break;
                        case 3:
                            encoder = new JpegBitmapEncoder();
                            break;
                        case 4:
                            encoder = new PngBitmapEncoder();
                            break;
                        case 5:
                            encoder = new TiffBitmapEncoder();
                            break;
                        default:
                            throw new ArgumentException("Unsupported image format");
                    }

                    encoder.Frames.Add(BitmapFrame.Create(memoryStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad));
                    encoder.Save(fileStream);
                }
            }
        }
    }
}
