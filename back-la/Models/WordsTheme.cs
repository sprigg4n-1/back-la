using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace back_la.Models;

public class WordsTheme
{

  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? id { get; set; }
  public string level { get; set; } = null!;
  public string theme { get; set; } = null!;

  [BsonElement("words")]
  [JsonPropertyName("words")]
  public MWord[] words { get; set; } = null!;
}

public class MWord
{
  public string sentence { get; set; } = null!;
  public string translate { get; set; } = null!;
  public string type { get; set; } = null!;
  public string word { get; set; } = null!;

}