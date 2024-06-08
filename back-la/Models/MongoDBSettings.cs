namespace back_la.Models;

public class MongoDBSettings
{
  public string ConnectionURI { get; set; } = null!;
  public string DatabaseName { get; set; } = null!;
  public string WordsThemeCollection { get; set; } = null!;
  public string UsersCollection { get; set; } = null!;
}