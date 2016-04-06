namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Collections.Specialized;
    using System.Net;
    using System.Text;
    using Newtonsoft.Json.Linq;

    public static class WebSequenceDiagrams
    {
        static WebSequenceDiagrams()
        {
            ServicePointManager.Expect100Continue = false;
        }

        public static WebSequenceDiagramsResult DownloadDiagram(String wsdScript, String style, String format)
        {
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

            WebSequenceDiagramsException.CheckResult(root);

            var img = root.GetValue("img");

            var imageData = webClient.DownloadData("http://www.websequencediagrams.com/" + img);

            return new WebSequenceDiagramsResult(imageData);
        }
    }
}
