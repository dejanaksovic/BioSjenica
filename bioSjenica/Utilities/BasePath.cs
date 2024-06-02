namespace bioSjenica.Utilities {
  public static class BasePath {
    public static string Get() {
      //Wizardry from the internet
      //Started this way now im stuck
      var directory = System.AppContext.BaseDirectory.Split(Path.DirectorySeparatorChar);
      var slice = new ArraySegment<string>(directory, 0, directory.Length - 4);
      var path = Path.Combine(slice.ToArray());
      return path;
    }
  }
}