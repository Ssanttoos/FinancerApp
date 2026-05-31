using Microsoft.AspNetCore.Mvc;
using FinanceiroApp.Data;
using FinanceiroApp.Models;

namespace FinanceiroApp.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly AppDbContext _db;

        public TransactionsController(AppDbContext db)
        {
            _db = db;
        }

        private int? GetUserId() => HttpContext.Session.GetInt32("UserId");

        // GET /Transactions - Lista todas as transações
        public IActionResult Index(string? type, string? category, int page = 1)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Auth");

            var query = _db.Transactions
                .Where(t => t.UserId == userId.Value)
                .AsQueryable();

            // Filtros
            if (type == "income") query = query.Where(t => t.Type == TransactionType.Income);
            if (type == "expense") query = query.Where(t => t.Type == TransactionType.Expense);
            if (!string.IsNullOrEmpty(category)) query = query.Where(t => t.Category == category);

            var transactions = query
                .OrderByDescending(t => t.Date)
                .ToList();

            ViewBag.SelectedType = type;
            ViewBag.SelectedCategory = category;
            ViewBag.Categories = GetCategories();

            return View(transactions);
        }

        // POST /Transactions/Create - Cria nova transação
        [HttpPost]
        public IActionResult Create(TransactionViewModel model)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Auth");

            var transaction = new Transaction
            {
                UserId = userId.Value,
                Description = model.Description,
                Amount = model.Amount,
                Type = model.Type,
                Category = model.Category,
                Date = model.Date
            };

            _db.Transactions.Add(transaction);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST /Transactions/Delete/5 - Remove uma transação
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Auth");

            var transaction = _db.Transactions
                .FirstOrDefault(t => t.Id == id && t.UserId == userId.Value);

            if (transaction != null)
            {
                _db.Transactions.Remove(transaction);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // Retorna lista de categorias disponíveis
        private List<string> GetCategories() => new()
        {
            "Salário", "Freelance", "Investimentos", "Outros (Receita)",
            "Alimentação", "Moradia", "Transporte", "Saúde",
            "Educação", "Lazer", "Roupas", "Tecnologia", "Outros (Despesa)"
        };
    }
}
