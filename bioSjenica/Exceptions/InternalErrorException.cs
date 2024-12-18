using bioSjenica.Utilities;

namespace bioSjenica.Exceptions {
  public class InternalErrorException : RequestException {
    public InternalErrorException(string message) : base(message, errorCodes.INTERNAL_ERROR) {}
  }
}