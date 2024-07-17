using Microsoft.AspNetCore.Mvc;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Controllers
{
    public class BPKBController : Controller
    {
        private readonly ApiService _apiService;
        public BPKBController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(InputBPKB inputBPKB)
        {
            //if (ModelState.IsValid)
            //{
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
            //}
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
