using bioSjenica.Utilities;

namespace bioSjenica.Exceptions {
  public class RequestException : Exception {
    public readonly string ErrorMessage;
    public readonly errorCodes StatusCode;
    public readonly IDictionary<string, object?>? ExtendedInformation;
    public RequestException(string ErrorMessage, errorCodes errorCode, IDictionary<string, object?>? extendedInformation = null)
      {
        this.ErrorMessage = ErrorMessage;
        this.StatusCode = errorCode;
        this.ExtendedInformation = extendedInformation;
      }
  }
}