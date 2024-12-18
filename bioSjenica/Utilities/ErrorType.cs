namespace bioSjenica.Utilities {
  public class ErrorType {
    errorCodes Status;
    string Message;
    public ErrorType(errorCodes status, string message) {
      Status = status;
      Message = message;
    }
  }
}