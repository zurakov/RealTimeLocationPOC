namespace RealTimeLocationPOC.Api.Brokers.Loggings
{
    public interface ILoggingBroker
    {
        void LogInformation(string message);
        void LogError(Exception exception);
    }
}
