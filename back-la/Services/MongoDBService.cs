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

  public async Task<List<MWord>> GetAllWordsFromAllThemes()
  {
    List<MWord> words = new List<MWord>();
    var themes = await _wordsTheme.Find(x => true).ToListAsync();
    foreach (var theme in themes)
    {
      words.AddRange(theme.words);
    }
    return words;
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

  // user tasks

  public async Task AddUserTask(string userId, UserTask newTask)
  {
    var update = Builders<User>.Update.Push(x => x.todo_list, newTask);
    await _usersCollection.UpdateOneAsync(x => x.id == userId, update);
  }

  public async Task DeleteUserTask(string userId, string taskId)
  {
    var update = Builders<User>.Update.PullFilter(x => x.todo_list, t => t.taskId == taskId);
    await _usersCollection.UpdateOneAsync(x => x.id == userId, update);
  }

  public async Task EditUserTask(string userId, UserTask updatedTask)
  {
    var filter = Builders<User>.Filter.And(
        Builders<User>.Filter.Eq(x => x.id, userId),
        Builders<User>.Filter.ElemMatch(x => x.todo_list, t => t.taskId == updatedTask.taskId)
    );

    var update = Builders<User>.Update.Set("todo_list.$", updatedTask);
    await _usersCollection.UpdateOneAsync(filter, update);
  }

  public async Task<List<UserTask>> GetUserTasks(string userId, string sortColumn = "Checked")
  {
    var user = await _usersCollection.Find(x => x.id == userId).FirstOrDefaultAsync();
    if (user == null || user.todo_list == null)
    {
      return new List<UserTask>();
    }

    var tasks = user.todo_list.AsQueryable();

    if (!string.IsNullOrEmpty(sortColumn))
    {
      // Сортування тільки за дозволеними колонками
      switch (sortColumn)
      {
        case "Date":
          tasks = tasks.OrderByDescending(t => t.date);
          break;
        case "Task":
          tasks = tasks.OrderBy(t => t.task);
          break;
        case "Important":
          tasks = tasks.OrderByDescending(t => t.important);
          break;
        case "Checked":
          tasks = tasks.OrderBy(t => t.done);
          break;
        default:
          break;
      }
    }

    return tasks.ToList();
  }

  // user words

  public async Task AddUserWord(string userId, MWord newWord)
  {
    var update = Builders<User>.Update.Push(x => x.words_to_learn, newWord);
    await _usersCollection.UpdateOneAsync(x => x.id == userId, update);
  }

  public async Task DeleteUserWord(string userId, string word)
  {
    var update = Builders<User>.Update.PullFilter(x => x.words_to_learn, w => w.word == word);
    await _usersCollection.UpdateOneAsync(x => x.id == userId, update);
  }

}