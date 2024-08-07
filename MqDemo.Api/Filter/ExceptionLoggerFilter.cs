using MassTransit;

namespace MqDemo.Api.Filter
{
    public class ExceptionLoggerFilter<T> : IFilter<ConsumeContext<T>> where T : class
    {
        private readonly ILogger _logger;

        long _exceptionCount;
        long _successCount;
        long _attemptCount;

        public void Probe(ProbeContext context)
        {
            var scope = context.CreateFilterScope("exceptionLogger");

            scope.Add("attempted", _attemptCount);
            scope.Add("succeeded", _successCount);
            scope.Add("faulted", _exceptionCount);
        }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            try
            {
                Interlocked.Increment(ref _attemptCount);

                await next.Send(context);

                Interlocked.Increment(ref _successCount);
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _exceptionCount);

                await Console.Out.WriteLineAsync($"An exception occurred: {ex.Message}");

                // propagate the exception up the call stack
                throw;
            }
        }
    }
}