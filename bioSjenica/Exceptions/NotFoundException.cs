namespace bioSjenica.Exceptions {
  public class NotFoundException : RequestException {
    public NotFoundException(string ObjectName):base($"{ObjectName} not found", 404){}
  }
}