using Microsoft.AspNetCore.Mvc;
using NZWalk.UI.Models.DTO;

namespace NZWalk.UI.Controllers
    {
    public class RegionController : Controller
        {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionController(IHttpClientFactory httpClientFactory)
            {
            this.httpClientFactory = httpClientFactory;
            }

        public async Task<IActionResult> Index()
            {
            List<RegionDTO> response = new List<RegionDTO>();
            try
                {
                // Get all region data
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7242/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();
                 response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());               
                }
            catch (Exception ex)
                {
                // log the error response
                throw;
                }

            return View(response);
            }


        public IActionResult Add() {
            return View();
            }
        }
    }
