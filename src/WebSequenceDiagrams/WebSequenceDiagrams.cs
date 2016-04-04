namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Windows.Media.Imaging;
    using Newtonsoft.Json.Linq;

    public static class WebSequenceDiagrams
    {
        public static BitmapImage GetDiagram(String wsdScript, String style, String format)
        {
            var bitmapData = DownloadDiagram(wsdScript, style, format);

            var bitmap = new BitmapImage();
            using (var stream = new MemoryStream(bitmapData))
            {
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
            }

            return bitmap;
        }

        private static Byte[] DownloadDiagram(String wsdScript, String style, String format)
        {
            ServicePointManager.Expect100Continue = false;

            var requestData = new NameValueCollection();
            requestData.Add("apiVersion", "1");
            requestData.Add("style", style);
            requestData.Add("format", format);
            requestData.Add("message", wsdScript);

            var webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var responseData =  webClient.UploadValues("http://www.websequencediagrams.com/index.php", "POST", requestData);
            var jsonText = Encoding.ASCII.GetString(responseData);

            var root = JObject.Parse(jsonText);

            var img = root.GetValue("img");

            return webClient.DownloadData("http://www.websequencediagrams.com/" + img);
        }
    }
}

