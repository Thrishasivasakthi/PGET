using Microsoft.AspNetCore.Mvc;
using MVC_UI.Models;

namespace MVC_UI.Controllers
{
    public class AccountController: Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("http://localhost:5202/api/user/login", new
            {
                email = model.Email,
                password = model.Password
            });

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid login.";
                return View();
            }

            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (content.Role != "Admin")
            {
                ViewBag.Error = "You are not authorized as Admin.";
                return View();
            }

            HttpContext.Session.SetString("JWToken", content.Token);
            return RedirectToAction("Dashboard", "Admin");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        class LoginResponse
        {
            public string Token { get; set; }
            public string Role { get; set; }
        }
    }
}
