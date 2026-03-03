using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommonTopicsLayout.Models;
using System.IO;

namespace CommonTopicsLayout.Controllers
{
    public class BlogController : Controller
    {
        private readonly EmeraldInsightsDbContext _context;

        public BlogController(EmeraldInsightsDbContext context)
        {
            _context = context;
        }

        // Fixes 404 on Startup
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Home");
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Home()
        {
            var posts = await _context.Articles.OrderByDescending(a => a.PublishedDate).Take(10).ToListAsync();
            return View(posts);
        }

        [Authorize]
        public async Task<IActionResult> Archive(string search)
        {
            var posts = _context.Articles.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                posts = posts.Where(a => a.Title.Contains(search) || a.Summary.Contains(search));

            return View(await posts.OrderByDescending(a => a.PublishedDate).ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Profile(string username)
        {
            string targetUser = username ?? User.Identity.Name;
            var userAccount = await _context.Users.FirstOrDefaultAsync(u => u.FullName == targetUser);
            var userPosts = await _context.Articles.Where(a => a.AuthorName == targetUser).OrderByDescending(a => a.PublishedDate).ToListAsync();

            ViewBag.Username = targetUser;
            ViewBag.Bio = userAccount?.Bio ?? "No bio shared yet.";
            ViewBag.PFP = userAccount?.ProfilePicturePath;
            return View(userPosts);
        }

        [Authorize]
        public async Task<IActionResult> Settings()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FullName == User.Identity.Name);
            return View(user);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSettings(string bio, IFormFile profilePhoto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FullName == User.Identity.Name);
            if (user != null)
            {
                user.Bio = bio;
                if (profilePhoto != null && profilePhoto.Length > 0)
                {
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(profilePhoto.FileName);
                    var filePath = Path.Combine(folderPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create)) { await profilePhoto.CopyToAsync(stream); }
                    user.ProfilePicturePath = "/uploads/" + fileName;
                }
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Profile", new { username = User.Identity.Name });
        }

        public IActionResult About() => View();
        public IActionResult Contact() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(string name, string email, string subject, string message)
        {
            var inquiry = new ContactMessage { Name = name, Email = email, Subject = subject, Message = message };
            _context.ContactMessages.Add(inquiry);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Inquiry logged successfully.";
            return RedirectToAction("Contact");
        }
    }
}