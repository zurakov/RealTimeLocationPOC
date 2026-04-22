namespace RealTimeLocationPOC.Api.Brokers.Loggings
{
    public class LoggingBroker : ILoggingBroker
    {
        private readonly ILogger<LoggingBroker> logger;

        public LoggingBroker(ILogger<LoggingBroker> logger)
        {
            this.logger = logger;
        }

        public void LogInformation(string message) =>
            this.logger.LogInformation(message);

        public void LogError(Exception exception) =>
            this.logger.LogError(exception, exception.Message);
    }
}
