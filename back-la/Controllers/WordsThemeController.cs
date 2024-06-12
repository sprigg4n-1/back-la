using back_la.Models;
using back_la.Services;
using Microsoft.AspNetCore.Mvc;

namespace back_la.Controllers;

[Controller]
[Route("api/[controller]")]
public class WordsThemeController : Controller
{
  private readonly MongoDbService _mongoDbService;
  public WordsThemeController(MongoDbService mongoDbService)
  {
    _mongoDbService = mongoDbService;
  }

  // words methods
  [HttpGet("{level}")]
  public async Task<List<WordsTheme>> Get(string level)
  {
    return await _mongoDbService.GetWordsTheme(level);
  }

  [HttpGet("{level}/{id}")]
  public async Task<WordsTheme> GetById(string id)
  {
    return await _mongoDbService.GetWordsThemesById(id);
  }

  // user methods

  [HttpGet("user/{id}")]
  public async Task<ActionResult<User>> GetUserById(string id)
  {
    var user = await _mongoDbService.GetUserById(id);
    if (user == null)
    {
      return NotFound();
    }

    return user;
  }
  [HttpGet("user/{email}/{password}")]
  public async Task<ActionResult<User>> GetUser(string email, string password)
  {
    var user = await _mongoDbService.GetUser(email, password);
    if (user == null)
    {
      return NotFound();
    }
    return user;
  }

  [HttpPost("user")]
  public async Task<IActionResult> AddUser([FromBody] User user)
  {
    await _mongoDbService.AddUser(user);
    return CreatedAtAction(nameof(GetUser), new { email = user.email, password = user.password }, user);
  }

  [HttpPut("user/{id}")]
  public async Task<IActionResult> UpdateUser(string id, [FromBody] User updatedUser)
  {
    var user = await _mongoDbService.GetUserById(id);

    if (user == null)
    {
      return NotFound();
    }

    updatedUser.id = user.id;
    await _mongoDbService.EditUser(id, updatedUser);
    return NoContent();
  }

  [HttpDelete("user/{id}")]
  public async Task<IActionResult> DeleteUser(string id)
  {
    var user = await _mongoDbService.GetUserById(id);
    if (user == null)
    {
      return NotFound();
    }

    await _mongoDbService.DeleteUser(id);
    return NoContent();
  }

  // user task methods
  [HttpPost("user/{id}/tasks")]
  public async Task<IActionResult> AddUserTask(string id, [FromBody] UserTask newTask)
  {
    await _mongoDbService.AddUserTask(id, newTask);
    return Ok();
  }

  [HttpPut("user/{userId}/tasks/{taskId}")]
  public async Task<IActionResult> EditUserTask(string userId, int taskId, [FromBody] UserTask updatedTask)
  {
    if (taskId != updatedTask.id)
    {
      return BadRequest("Task ID does not match the ID in the updated task.");
    }

    await _mongoDbService.EditUserTask(userId, updatedTask);
    return Ok();
  }

  [HttpDelete("user/{userId}/tasks/{taskId}")]
  public async Task<IActionResult> DeleteUserTask(string userId, int taskId)
  {
    await _mongoDbService.DeleteUserTask(userId, taskId);
    return Ok();
  }

  // user words methods
  [HttpPost("user/{id}/words")]
  public async Task<IActionResult> AddUserWord(string id, [FromBody] MWord newWord)
  {
    await _mongoDbService.AddUserWord(id, newWord);
    return Ok();
  }

  [HttpDelete("user/{userId}/words/{word}")]
  public async Task<IActionResult> DeleteUserWord(string userId, string word)
  {
    await _mongoDbService.DeleteUserWord(userId, word);
    return Ok();
  }

}