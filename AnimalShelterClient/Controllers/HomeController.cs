using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AnimalShelterClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace AnimalShelterClient.Controllers;
[Authorize]
public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;

  public HomeController(ILogger<HomeController> logger)
  {
    _logger = logger;
  }
  [AllowAnonymous]
  public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
  {
    List<Animal> animalList = new List<Animal> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"http://localhost:5000/api/Animals?&page={page}&pageSize={pageSize}"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray animalArray = (JArray)jsonResponse["data"];
        animalList = animalArray.ToObject<List<Animal>>();
      }
    }

    List<Animal> animalList2 = new List<Animal> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync("http://localhost:5000/api/Animals?&page=1&pageSize=1001"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse2 = JObject.Parse(apiResponse);
        JArray animalArray2 = (JArray)jsonResponse2["data"];
        animalList2 = animalArray2.ToObject<List<Animal>>();
      }
    }

    ViewBag.LastId = animalList2.Count();
    
    ViewBag.CurrentPage = page;
    ViewBag.PageSize = pageSize;
    
    return View(animalList);
  }
  public IActionResult Privacy()
  {
    return View();
  }

  [HttpPost]
  public ActionResult Create(Animal animal, int currentPage, int pageSize)
  {
    Animal.Post(animal);
    return RedirectToAction("Index", new { page = currentPage, pageSize = pageSize });
  }

  public async Task<ActionResult> LoadEdit(int id)
  {
    List<Animal> animalList = new List<Animal> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"http://localhost:5000/api/Animals?id={id}"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray animalArray = (JArray)jsonResponse["data"];
        animalList = animalArray.ToObject<List<Animal>>();
      }
    }
    Animal animal = animalList[0];
    return RedirectToAction("Edit", animal);
  }

  [HttpPost, ActionName("Edit")]
  public ActionResult PutEdit(Animal animal, int currentPage, int pageSize)
  {
    Animal.Put(animal);
    // return RedirectToAction("Index");
    return RedirectToAction("Index", new { page = currentPage, pageSize = pageSize } );
  }

  [HttpPost, ActionName("Delete")]
  public ActionResult DeleteConfirmed(int id, int currentPage, int pageSize)
  {
    Animal.Delete(id);
    return RedirectToAction("Index", new { page = currentPage, pageSize = pageSize } );
  }
}