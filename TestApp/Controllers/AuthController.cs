using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiService;
        public AuthController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO userLogin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _apiService.PostAsync<string>("api/Auth/login", userLogin);
                    HttpContext.Session.SetString("UserName", userLogin.UserName);
                    return RedirectToAction("Create", "Bpkb");
                }
                catch (HttpRequestException)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(userLogin);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }


    }
}
