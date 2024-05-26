namespace Common;

public class MediWebClientException : Exception
{
    public MediWebFeature MediWebFeature { get; set; }
    public string ExceptionMessage {  get; set; }

    public MediWebClientException(MediWebFeature mediWebFeature, string exceptionMessage)
    {
        MediWebFeature = mediWebFeature;
        ExceptionMessage = exceptionMessage;
    }
}
