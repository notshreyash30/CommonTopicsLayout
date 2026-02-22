using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommonTopicsLayout.Models;

namespace CommonTopicsLayout.Controllers
{
    public class AccountController : Controller
    {
        private readonly EmeraldInsightsDbContext _context;

        public AccountController(EmeraldInsightsDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // REGISTRATION LOGIC
        // ==========================================

        [HttpGet]
        public IActionResult Register()
        {
            // If already logged in, don't show register page
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Home", "Blog");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if email is already taken
                var exists = await _context.Users.AnyAsync(u => u.Email == user.Email);
                if (exists)
                {
                    ModelState.AddModelError("Email", "This email is already in our circle.");
                    return View(user);
                }

                user.CreatedAt = DateTime.Now;
                _context.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }
            return View(user);
        }

        // ==========================================
        // LOGIN LOGIC
        // ==========================================

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Home", "Blog");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("UserId", user.Id.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "EmeraldAuth");

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                    };

                    await HttpContext.SignInAsync("EmeraldAuth",
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Home", "Blog");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        // ==========================================
        // PASSWORD RECOVERY
        // ==========================================

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                user.ResetToken = Guid.NewGuid().ToString();
                user.ResetTokenExpiry = DateTime.Now.AddMinutes(30);
                _context.Update(user);
                await _context.SaveChangesAsync();

                // For your BCS project, we display the link on screen instead of emailing
                ViewBag.ResetLink = "/Account/ResetPassword?token=" + user.ResetToken;
            }
            else
            {
                ViewBag.Error = "Email not found.";
            }
            return View();
        }

        // ==========================================
        // LOGOUT LOGIC
        // ==========================================

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("EmeraldAuth");
            return RedirectToAction("Index", "Blog");
        }
    }
}