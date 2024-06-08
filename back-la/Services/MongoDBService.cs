using back_la.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace back_la.Services;

public class MongoDbService
{

  private readonly IMongoCollection<WordsTheme> _wordsTheme;
  private readonly IMongoCollection<User> _usersCollection;

  public MongoDbService(IOptions<MongoDBSettings> mongoDBSettings)
  {
    MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
    IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
    _wordsTheme = database.GetCollection<WordsTheme>(mongoDBSettings.Value.WordsThemeCollection);
    _usersCollection = database.GetCollection<User>(mongoDBSettings.Value.UsersCollection);
  }


  // words_theme collection
  public async Task<List<WordsTheme>> GetWordsTheme(string level)
  {
    return await _wordsTheme.Find(x => x.level.Equals(level.ToUpper())).ToListAsync();
  }

  public async Task<WordsTheme> GetWordsThemesById(string id)
  {
    WordsTheme theme = await _wordsTheme.Find(x => x.id == id).FirstOrDefaultAsync();
    return theme;
  }

  // user collection
  public async Task<User> GetUser(string email, string password)
  {
    User user = await _usersCollection.Find(x => x.email == email && x.password == password).FirstOrDefaultAsync();
    return user;
  }

  public async Task<User> GetUserById(string id)
  {
    return await _usersCollection.Find(x => x.id == id).FirstOrDefaultAsync();
  }



  public async Task AddUser(User user)
  {
    await _usersCollection.InsertOneAsync(user);
  }

  public async Task EditUser(string id, User updatedUser)
  {
    await _usersCollection.ReplaceOneAsync(x => x.id == id, updatedUser);
  }

  public async Task DeleteUser(string id)
  {
    await _usersCollection.DeleteOneAsync(x => x.id == id);
  }

  public async Task<User> GetUserTasks(string id)
  {
    User user = await _usersCollection.Find(x => x.id == id).FirstOrDefaultAsync();
    return user;
  }

  public async void AddUserTask(string id)
  {

  }

  public async void DeleteUserTask(string id)
  {

  }

  public async void DeleteEditTask(string id)
  {

  }

  public async Task<User> GetUserWords(string id)
  {
    User user = await _usersCollection.Find(x => x.id == id).FirstOrDefaultAsync();
    return user;
  }

  public async void AddUserWord(string id)
  {

  }

  public async void DeleteUserWord(string id)
  {

  }

}