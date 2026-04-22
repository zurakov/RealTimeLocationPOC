namespace RealTimeLocationPOC.Web.Brokers.BroadCasts
{
    public interface IBroadCastBroker
    {
        ValueTask<Stream> GetRawStreamAsync(Guid businessId);
    }
}
