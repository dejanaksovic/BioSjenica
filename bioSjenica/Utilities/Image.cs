namespace bioSjenica.Utilities {
  public static class Image {
    public static string basePath = BasePath.Get();
    public static async Task<string> Create(string folderName, string ImageName, IFormFile image) {
      // All the checking again?
      if(!image.ContentType.StartsWith("image")) {
        //It's not an image
        return "fail/The document is not of type image";
      }
      var mimeType = image.ContentType.Split("/")[1];
      var fullPath = $"{basePath}/wwwroot/images/{folderName}/{ImageName}.{mimeType}";

      using(FileStream stream = File.Create(fullPath)) {
        try {
          await image.CopyToAsync(stream);
          return $"success//{folderName}/{ImageName}.{mimeType}";
        }
        catch(Exception e) {
          return "fail//Image was not able to be saved";
        }
      }
    }
    public static void Delete(string imageUrl) {
      Console.WriteLine($"I do be existing and i do be deleting with info {basePath}/images/{imageUrl}");
      if(imageUrl == "no-image.png") {
        return;
      }
      if(File.Exists($"{basePath}/wwwroot/images/{imageUrl}")) {
        File.Delete($"{basePath}/wwwroot/images/{imageUrl}");
      }
    }
    public static string? Move(string oldUrl, string newUrl) {
      
      if(File.Exists($"{basePath}/images/{oldUrl}")) {
        File.Move($"{basePath}/images/{oldUrl}", $"{basePath}/images/{newUrl}");
        return $"{basePath}/images/{newUrl}";
      }
      return null;
    }
    public static bool Exists(string folderName, string imageUrl) {
      return File.Exists($"{basePath}/images/{imageUrl}") ? true : false;
    }
  }
}