using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tutorials.ApiCall
{
    public interface IHttpClient : IDisposable
    {
        Task<string> Get(Uri url, CancellationToken cancellationToken = default);
        Task<string> Post(Uri url, string data, CancellationToken cancellationToken = default);
        Task<string> Put(Uri url, string data, CancellationToken cancellationToken = default);
        Task<string> Put(Uri url, CancellationToken cancellationToken = default);
    }
}