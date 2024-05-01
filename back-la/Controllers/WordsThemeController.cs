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

}