using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceiroApp.Data;
using FinanceiroApp.Models;

namespace FinanceiroApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        // Verifica se o usuário está logado e retorna o ID
        private int? GetUserId() => HttpContext.Session.GetInt32("UserId");

        // GET /Home/Index - Dashboard principal
        public IActionResult Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Auth");

            var user = _db.Users.Find(userId.Value);
            if (user == null) return RedirectToAction("Login", "Auth");

            // Transações do mês atual
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);

            var transactions = _db.Transactions
                .Where(t => t.UserId == userId.Value)
                .OrderByDescending(t => t.Date)
                .ToList();

            var monthTransactions = transactions
                .Where(t => t.Date >= startOfMonth)
                .ToList();

            // Calcula totais do mês
            var totalIncome = monthTransactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            var totalExpenses = monthTransactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            // Despesas por categoria (para o gráfico de pizza)
            var expensesByCategory = monthTransactions
                .Where(t => t.Type == TransactionType.Expense)
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));

            // Dados mensais dos últimos 6 meses (para o gráfico de barras)
            var monthlyData = new List<MonthlyData>();
            for (int i = 5; i >= 0; i--)
            {
                var month = now.AddMonths(-i);
                var start = new DateTime(month.Year, month.Month, 1);
                var end = start.AddMonths(1);

                var income = transactions
                    .Where(t => t.Date >= start && t.Date < end && t.Type == TransactionType.Income)
                    .Sum(t => t.Amount);

                var expenses = transactions
                    .Where(t => t.Date >= start && t.Date < end && t.Type == TransactionType.Expense)
                    .Sum(t => t.Amount);

                monthlyData.Add(new MonthlyData
                {
                    Month = month.ToString("MMM/yy"),
                    Income = income,
                    Expenses = expenses
                });
            }

            var goals = _db.Goals
                .Where(g => g.UserId == userId.Value)
                .ToList();

            var viewModel = new DashboardViewModel
            {
                User = user,
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                Balance = totalIncome - totalExpenses,
                RecentTransactions = transactions.Take(8).ToList(),
                Goals = goals,
                ExpensesByCategory = expensesByCategory,
                MonthlyData = monthlyData
            };

            return View(viewModel);
        }
    }
}
