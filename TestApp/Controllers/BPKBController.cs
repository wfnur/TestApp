using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Controllers
{
    public class BPKBController : Controller
    {
        private readonly ApiService _apiService;
        private readonly HttpClient _httpClient;
        public BPKBController(ApiService apiService, HttpClient httpClient)
        {
            _apiService = apiService;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var location = await _apiService.GetLocationAsync("api/Location");
            var model = new InputBPKB
            {
                Locations = location
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InputBPKB inputBPKB)
        {
            try
            {
                inputBPKB.created_by = "USSER";
                inputBPKB.last_updated_by = "USSER";
                var result = await _apiService.PostAsync<string>("api/BPKB/insert", inputBPKB);
                if (result != null)
                {
                    return RedirectToAction("Create", "Bpkb");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while creating the BPKB.");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "Failed");
            }
            return View(inputBPKB);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bpkbs = await _apiService.GetBpkbsAsync("api/BPKB");
            return View(bpkbs);
        }
    }
}
