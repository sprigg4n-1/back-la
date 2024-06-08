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

}