using Microsoft.AspNetCore.Mvc;
using NZWalk.UI.Models.View_Model;
using System.Text.Json;
using System.Text;
using NZWalk.UI.Models.DTO;

namespace NZWalk.UI.Controllers
    {
    public class RegionController : Controller
        {
        private readonly IHttpClientFactory httpClientFactory;

        // Constructor: Injects the IHttpClientFactory to make HTTP requests
        public RegionController(IHttpClientFactory httpClientFactory)
            {
            this.httpClientFactory = httpClientFactory;
            }

        // GET: Display all regions
        public async Task<IActionResult> Index()
            {
            List<RegionDTO> response = new List<RegionDTO>();
            try
                {
                // Fetch data from API
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7242/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());
                }
            catch (Exception ex)
                {
                // Log the error
                throw;
                }
            return View(response); // Return list of regions to the view
            }

        // GET: Add a new region (show form)
        [HttpGet]
        public IActionResult Add()
            {
            return View(); 
            }

        // POST: Add a new region (submit form)
        [HttpPost]
        public async Task<IActionResult> Add(CreateRegionVM createRegionVM)
            {
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage
                {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7242/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(createRegionVM), Encoding.UTF8, "application/json")
                };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode(); 
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();
            if (response != null)
                {
                return RedirectToAction("Index", "Region"); 
                }
            return View(); // Return to form if something goes wrong
            }

        // GET: Edit an existing region (load form)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
            {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<RegionDTO>($"https://localhost:7242/api/regions/{id}");
            if (response != null)
                {
                return View(response); // Pass the region data to the edit view
                }
            return View(null); // Return empty if region not found
            }

        // POST: Update an existing region
        [HttpPost]
        public async Task<IActionResult> Edit(RegionDTO regionDTO)
            {
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage
                {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7242/api/regions/{regionDTO.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(regionDTO), Encoding.UTF8, "application/json")
                };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode(); 
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();
            if (response != null)
                {
                return RedirectToAction("Index", "Region"); 
                }
            return View();
            }

        // POST: Delete an existing region
        [HttpPost]
        public async Task<IActionResult> Delete(RegionDTO regionDTO)
            {
            try
                {
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7242/api/regions/{regionDTO.Id}");
                httpResponseMessage.EnsureSuccessStatusCode(); 
                return RedirectToAction("Index", "Region"); 
                }
            catch (Exception ex)
                {
                // Handle exception
                }
            return View("index"); 
            }
        }
    }
