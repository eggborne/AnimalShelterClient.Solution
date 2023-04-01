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
  public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
  {
    List<Animal> animalList = new List<Animal> { };
    int totalCount;
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"http://localhost:5000/api/Animals?&page={page}&pageSize={pageSize}"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray animalArray = (JArray)jsonResponse["data"];
        totalCount = (int)(JValue)jsonResponse["totalCount"];
        animalList = animalArray.ToObject<List<Animal>>();
      }
    }
    var totalPages = (totalCount % pageSize) == 0 ? (totalCount / pageSize) : Math.Round((decimal)(totalCount / pageSize)) + 1;
    ViewBag.TotalPages = totalPages;
    ViewBag.TotalCount = totalCount;
    ViewBag.CurrentPage = page;
    ViewBag.PageSize = pageSize;
    int distanceFromEnd = (int)(totalPages) - page;
    if (page <= 2) {
      ViewBag.StartingPageLinkIndex = 1;
    } else if (page >= totalPages - 1) {
      ViewBag.StartingPageLinkIndex = ViewBag.CurrentPage - (4 - distanceFromEnd);
      if (ViewBag.StartingPageLinkIndex <= 0) {
        ViewBag.StartingPageLinkIndex = 1;
      }
    } else {
      ViewBag.StartingPageLinkIndex =  ViewBag.CurrentPage - 2;
    }    
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