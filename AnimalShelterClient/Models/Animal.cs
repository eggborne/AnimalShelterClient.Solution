using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AnimalShelterClient.Models
{
  public class Animal
  {
    public int AnimalId { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public string Gender { get; set; }
    public string Name { get; set; }

    public static Animal[] GetAnimals()
    {
      Task<string> apiCallTask = ApiHelper.GetAll();
      string result =  apiCallTask.Result;

      JObject jsonResponse = JObject.Parse(result);
      List<Animal> animalList = JsonConvert.DeserializeObject<List<Animal>>(jsonResponse["data"].ToString());      
      return animalList.ToArray();
    }

    public static Animal GetDetails(int id)
    {
      var apiCallTask = ApiHelper.Get(id);
      var result = apiCallTask.Result;

      JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(result);
      Animal animal = JsonConvert.DeserializeObject<Animal>(jsonResponse.ToString());

      return animal;
    }

    public static void Post(Animal animal)
    {
      string jsonAnimal = JsonConvert.SerializeObject(animal);
      ApiHelper.Post(jsonAnimal);
    }

    public static void Put(Animal animalToEdit)
    {
      string jsonAnimal = JsonConvert.SerializeObject(animalToEdit);
      ApiHelper.Put(animalToEdit.AnimalId, jsonAnimal);
    }

    public static void Delete(int id)
    {
      ApiHelper.Delete(id);
    }
  }
}