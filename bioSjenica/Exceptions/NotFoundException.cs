using bioSjenica.Utilities;

namespace bioSjenica.Exceptions {
  public class NotFoundException : RequestException {
    public NotFoundException(string ObjectName):base($"{ObjectName} not found", errorCodes.NOT_FOUND){}
  }
}