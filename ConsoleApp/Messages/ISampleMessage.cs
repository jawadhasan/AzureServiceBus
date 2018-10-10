namespace Messages
{
    public interface ISampleMessage
    {
        string CorrelationId { get; }
        string Message { get; }
    }
}
