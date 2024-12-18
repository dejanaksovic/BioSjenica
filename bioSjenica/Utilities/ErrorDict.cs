namespace bioSjenica.Utilities {
  public static class ErrorDict {
    public static Dictionary<string, object?> CreateDict(string name, object? value) {
      return new Dictionary<string, object?> { { name, value } };
    }
  }
}