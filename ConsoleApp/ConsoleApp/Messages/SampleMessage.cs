using System;
using Messages;

namespace ConsoleApp.Messages
{
    public class SampleMessage : ISampleMessage
    {
        public string CorrelationId { get; }
        public string Message { get; }

        public SampleMessage(string message)
        {
            CorrelationId = Guid.NewGuid().ToString("N");
            Message = message;
        }
    }
}
