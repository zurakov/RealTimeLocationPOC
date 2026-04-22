using RealTimeLocationPOC.Web.Models;

namespace RealTimeLocationPOC.Web.Brokers.BroadCasts
{
    public class BroadCastBroker : IBroadCastBroker
    {
        private readonly HttpClient httpClient;
        private readonly string apiBaseUrl = "http://localhost:5202/api/businesses";

        public BroadCastBroker(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.Timeout = Timeout.InfiniteTimeSpan;
        }

        public async ValueTask<Stream> GetRawStreamAsync(Guid businessId)
        {
            string url = $"{apiBaseUrl}/{businessId}/stream";
            
            return await this.httpClient.GetStreamAsync(url);
        }
    }
}
