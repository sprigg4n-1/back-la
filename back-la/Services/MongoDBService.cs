using back_la.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace back_la.Services;

public class MongoDbService
{

  private readonly IMongoCollection<WordsTheme> _wordsTheme;

  public MongoDbService(IOptions<MongoDBSettings> mongoDBSettings)
  {
    MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
    IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
    _wordsTheme = database.GetCollection<WordsTheme>(mongoDBSettings.Value.WordsThemeCollection);
  }

  public async Task<List<WordsTheme>> GetWordsTheme(string level)
  {
    return await _wordsTheme.Find(x => x.level.Equals(level.ToUpper())).ToListAsync();
  }

  public async Task<WordsTheme> GetWordsThemesById(String id)
  {
    WordsTheme theme = await _wordsTheme.Find(x => x.id == id).FirstOrDefaultAsync();
    return theme;
  }
}