using System.Net.Http;
using System.Threading.Tasks;

namespace IocDemo.BLL
{
    public class DownloadService : IDownloadService
    {
        public Task<string> DownloadHtml(string url)
        {
            HttpClient httpClient = new HttpClient();
            return httpClient.GetAsync(url).ContinueWith(task =>
           {
               return task.Result.Content.ReadAsStringAsync().Result;
           });

        }
    }
}
