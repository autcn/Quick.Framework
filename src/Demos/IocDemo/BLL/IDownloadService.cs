using System.Threading.Tasks;

namespace IocDemo.BLL
{
    public interface IDownloadService
    {
        Task<string> DownloadHtml(string url);
    }
}
