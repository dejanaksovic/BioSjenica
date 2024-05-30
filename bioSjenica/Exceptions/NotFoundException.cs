namespace bioSjenica.Exceptions {
  public class NotFoundException : RequestException {
    public NotFoundException(object objectNotFound):base($"${nameof(objectNotFound)} not found", 404){}
  }
}