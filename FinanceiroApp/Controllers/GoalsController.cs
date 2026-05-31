using Microsoft.AspNetCore.Mvc;
using FinanceiroApp.Data;
using FinanceiroApp.Models;

namespace FinanceiroApp.Controllers
{
    public class GoalsController : Controller
    {
        private readonly AppDbContext _db;

        public GoalsController(AppDbContext db)
        {
            _db = db;
        }

        private int? GetUserId() => HttpContext.Session.GetInt32("UserId");

        // GET /Goals - Lista metas
        public IActionResult Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Auth");

            var goals = _db.Goals
                .Where(g => g.UserId == userId.Value)
                .OrderBy(g => g.Deadline)
                .ToList();

            return View(goals);
        }

        // POST /Goals/Create - Cria nova meta
        [HttpPost]
        public IActionResult Create(GoalViewModel model)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Auth");

            var goal = new Goal
            {
                UserId = userId.Value,
                Title = model.Title,
                Description = model.Description,
                TargetAmount = model.TargetAmount,
                CurrentAmount = model.CurrentAmount,
                Deadline = model.Deadline
            };

            _db.Goals.Add(goal);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST /Goals/AddAmount - Adiciona valor à meta
        [HttpPost]
        public IActionResult AddAmount(int id, decimal amount)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Auth");

            var goal = _db.Goals.FirstOrDefault(g => g.Id == id && g.UserId == userId.Value);
            if (goal != null)
            {
                goal.CurrentAmount = Math.Min(goal.TargetAmount, goal.CurrentAmount + amount);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // POST /Goals/Delete/5
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Auth");

            var goal = _db.Goals.FirstOrDefault(g => g.Id == id && g.UserId == userId.Value);
            if (goal != null)
            {
                _db.Goals.Remove(goal);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
