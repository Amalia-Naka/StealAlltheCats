using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.DAL.Data;
using StealAllTheCats.DAL.Helpers;
using StealAllTheCats.DAL.Models;
using System.Text.Json;

namespace StealAllTheCats.Application.Services
{
    public class CatService : ICatService
    {
        private readonly HttpClient _httpClient; //http client for API
        private readonly IDbExtension _dbHandler; //data handling
        private readonly ILogger<CatService> _logger; // Logger for errors and events

        public CatService(HttpClient httpClient, IDbExtension dbHandler, IConfiguration config, ILogger<CatService> logger) //DI
        {
            _httpClient = httpClient;
            _dbHandler = dbHandler;
            _logger = logger;

            //to fetch more than 10 images and to use breeds we need ApiKey
            var apiKey = config["CatApiKey"];
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
            }
        }

        public async Task<List<Cat>> FetchCats()
        {
            var StolenCats = new List<Cat>();

            try
            {
                //_dbHandler.LoadAllCats(); //load all the cats from the database

                //the catApi documantation Portl
                var url = $"https://api.thecatapi.com/v1/images/search?has_breeds=1&limit=25";
                var response = await _httpClient.GetAsync(url); //cats response http get to api

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to fetch data from Cat API: {StatusCode}", response.StatusCode);
                    return StolenCats;
                }

                var json = await response.Content.ReadAsStringAsync(); //take the content from response and make it to string
                //Console.WriteLine($" JSON from API:\n{json}");

                var data = JsonSerializer.Deserialize<List<CatApiResponse>>(json); //turn json string to list of catapi objectd
                if (data == null || data.Count == 0)
                {
                    _logger.LogWarning("Empty or invalid data");
                    return StolenCats;
                }


                //processing ever cat
                foreach (var catApi in data)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(catApi.Id)) { continue; } //check if string is empty
                        if (await _dbHandler.CatExists(catApi.Id)) { continue; } //check whether cat already exists if yes -> next cat


                        //creat Cat etity from catApi
                        var cat = new Cat
                        {
                            CatId = catApi.Id,
                            Width = catApi.width,
                            Height = catApi.height,
                            Image = catApi.url,
                            Created = DateTime.UtcNow,
                            CatTags = new List<CatTag>()
                        };
                        //Need temperament information from breed
                        Breed? breed = catApi.Breeds.FirstOrDefault(); //take breed 
                        if (breed != null && !string.IsNullOrWhiteSpace(breed.Temperament))
                        {
                            //temperament is a string, separate to temps
                            var temps = breed.Temperament.Split(',').Select(t => t.Trim());

                            foreach (var tempName in temps)
                            {
                                //check whether temperament exists in db if not add it
                                var temp = await _dbHandler.GetTagByName(tempName);

                                if (temp == null)
                                {
                                    temp = new Tag
                                    {
                                        Name = tempName,
                                        Created = DateTime.UtcNow
                                    };
                                    await _dbHandler.AddTag(temp);
                                }

                                //create connection object CatTag
                                var catTag = new CatTag
                                {
                                    Cat = cat,
                                    Tag = temp
                                };
                                cat.CatTags.Add(catTag);
                            }
                        }
                        await _dbHandler.AddCat(cat);
                        StolenCats.Add(cat);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing cat with ID: {CatId}", catApi.Id);
                    }
                }
                await _dbHandler.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error could not fetch the cats from API");
            }
            return StolenCats;
        }


        //Fetches the cat with a specific Id from db
        public async Task<Cat?> GetCatById(int id)
        {
            Cat? cat = null;
            if (id <= 0)
            {
                _logger.LogWarning("Invalid cat id requested: {Id}", id);
                return null;
            }
            try
            {
                cat = await _dbHandler.GetCatById(id);

                if (cat == null)
                {
                    _logger.LogWarning("No cat was found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cat by ID: {Id}", id);
            }
            return cat;
        }


        //gets list of cats based on pagination 
        public async Task<List<Cat>> GetCatsByPaging(int page, int pageSize)
        {
            List<Cat> StolenCats = new List<Cat>();

            if (page <= 0) { page = 1; }
            if (pageSize <= 0) { pageSize = 10; }

            try
            {
                StolenCats = await _dbHandler.GetCatByPaging(page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Cats by paging");
            }
            return StolenCats;
        }

        //gets list on of cats based on an temperament and pagination
        public async Task<List<Cat>> GetCatsByTagPaging(int page, int pageSize, string tagName)
        {
            {
                List<Cat> StolenCtas = new List<Cat>();

                if (string.IsNullOrWhiteSpace(tagName)) //if tagName is not given -> warning
                {
                    _logger.LogWarning("Tag is missing in query parameters");

                }

                if (page <= 0) { page = 1; }
                if (pageSize <= 0) { pageSize = 10; }

                try
                {
                    StolenCtas = await _dbHandler.GetCatsByTagPaging(page, pageSize, tagName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching Cats by tagName");
                }
                return StolenCtas;
            }



        }
    }
}
    
