namespace bioSjenica.Exceptions {
  public class RequestException : Exception {
    public readonly string ErrorMessage;
    public readonly int StatusCode;
    public RequestException(string ErrorMessage, int StatusCode)
      {
        this.ErrorMessage = ErrorMessage;
        this.StatusCode = StatusCode;
      }
  }
}