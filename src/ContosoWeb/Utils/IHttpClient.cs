using System.Threading.Tasks;

namespace ContosoWeb.Utils
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string uri);
    }
}