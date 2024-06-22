namespace back_la.Models;

using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


public class User
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? id { get; set; }
  public string nickname { get; set; }
  public string email { get; set; }
  public string password { get; set; }
  public string description { get; set; }
  public string accountPhoto { get; set; }

  [BsonElement("words_to_learn")]
  [JsonPropertyName("words_to_learn")]
  public MWord[] words_to_learn { get; set; }

  [BsonElement("todo_list")]
  [JsonPropertyName("todo_list")]
  public UserTask[] todo_list { get; set; }
}

public class UserTask
{
  public string taskId { get; set; }
  public DateTime date { get; set; }
  public string task { get; set; }
  public bool done { get; set; }
  public bool important { get; set; }
}
