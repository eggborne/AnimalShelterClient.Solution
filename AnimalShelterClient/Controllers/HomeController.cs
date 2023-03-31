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
  public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
  {
    List<Animal> animalList = new List<Animal> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"http://localhost:5000/api/Animals?question=false&page={page}&pageSize={pageSize}"))
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
      using (var response = await httpClient.GetAsync("http://localhost:5000/api/Animals?question=false&page=1&pageSize=1001"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray animalArray = (JArray)jsonResponse["data"];
        animalList2 = animalArray.ToObject<List<Animal>>();
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

  public ActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public ActionResult Create(Animal animal)
  {
    Animal.Post(animal);
    return RedirectToAction("Index");
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

  public ActionResult Edit(Animal animal)
  {
    return View(animal);
  }

  [HttpPost, ActionName("Edit")]
  public ActionResult PutEdit(Animal animal)
  {
    Animal.Put(animal);
    return RedirectToAction("Index");
  }

  public ActionResult Delete(int id, int currentPage)
  {
    Animal animal = Animal.GetDetails(id);
    ViewBag.CurrentPage = currentPage;
    return View(animal);
  }

  [HttpPost, ActionName("Delete")]
  public ActionResult DeleteConfirmed(int id, int currentPage)
  {
    Animal.Delete(id);
    return RedirectToAction("Index", new { page = currentPage, pageSize = 10 } );
  }
}