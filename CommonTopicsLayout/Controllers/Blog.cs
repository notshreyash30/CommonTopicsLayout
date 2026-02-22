using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommonTopicsLayout.Models;

namespace CommonTopicsLayout.Controllers
{
    public class BlogController : Controller
    {
        private readonly EmeraldInsightsDbContext _context;

        public BlogController(EmeraldInsightsDbContext context)
        {
            _context = context;
        }

        // Public Landing Page
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Home");
            return View();
        }

        // Private Dashboard - Displays recent posts
        [Authorize]
        public async Task<IActionResult> Home()
        {
            // Fetch top 5 most recent articles
            var recentArticles = await _context.Articles
                .OrderByDescending(a => a.PublishedDate)
                .Take(5)
                .ToListAsync();

            return View(recentArticles);
        }

        // Article Archive (Card Grid)
        [Authorize]
        public async Task<IActionResult> Articles()
        {
            var list = await _context.Articles
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();
            return View(list);
        }

        // Archive List (Table View)
        [Authorize]
        public async Task<IActionResult> Archive()
        {
            var list = await _context.Articles
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();
            return View(list);
        }

        // Create Article - Show Form
        [Authorize]
        public IActionResult Create() => View();

        // Create Article - Save to Database
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                article.PublishedDate = DateTime.Now;
                article.AuthorName = User.Identity.Name;

                _context.Articles.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Home)); // Redirect to Dashboard after posting
            }
            return View(article);
        }

        public IActionResult About() => View();
        public IActionResult Contact() => View();
    }
}