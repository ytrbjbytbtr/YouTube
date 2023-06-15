
using System;
    using System.Net;

    namespace YouTubeGetUri
    {
        class Program
        {
            static void Main()
            {
                var urlVideo = "https://www.youtube.com/watch?v=glJH30MyHBI&t=1s";
                var serviceLinkResult = "https://catch.tube/result?url=";
                var getUrl = serviceLinkResult + urlVideo;
                var webClient = new WebClient();
                var uri = new Uri(getUrl);
                Console.WriteLine("Выполняется запуск загрузки...");
                webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
                webClient.DownloadStringAsync(uri);
                Console.ReadLine();
            }

            private static void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
            {
                Console.WriteLine("Загрузка завершена!");
                var serviceLink = "https://catch.tube";
                var result = e.Result;
                var href = result.Split("href");
                var get = GetServiceUri(href);
                var fullServiceLink = serviceLink + get;
                var request = (HttpWebRequest)WebRequest.Create(fullServiceLink);
                request.AllowAutoRedirect = false;
                var response = (HttpWebResponse)request.GetResponse();
                var uri = response.Headers["Location"];
                response.Close();
                Console.Write("Прямая ссылка на видео: " + uri);
            }

            private static string GetServiceUri(string[] data)
            {
                foreach (var get in data)
                {
                    if (get.IndexOf("Only Audio") > 0 && get.IndexOf("w/o Audio") == -1 && get.IndexOf("/get?") > 0)
                    {
                        var indexStart = get.IndexOf("\"") + 1;
                        var newGet = get.Substring(indexStart);
                        var indexEnd = newGet.IndexOf("\"");
                        newGet = newGet.Substring(0, indexEnd);
                        newGet = newGet.Replace("amp;", "");
                        newGet = newGet.Replace(" ", "%20");
                        newGet = newGet.Replace("#039;", "%27");
                        return newGet;
                    }
                }
                return null;
            }

            private static void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
            {
                Console.WriteLine("Процесс загрузки страницы: " + e.ProgressPercentage + "%");
            }
        }
    }

