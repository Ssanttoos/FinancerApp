// ==============================================
// MODELS - Representam as tabelas do banco de dados
// ==============================================

namespace FinanceiroApp.Models
{
    // Usuário do sistema
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = ""; // Senha criptografada
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Um usuário tem muitas transações e metas
        public List<Transaction> Transactions { get; set; } = new();
        public List<Goal> Goals { get; set; } = new();
    }

    // Transação financeira (despesa ou receita)
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; } = "";
        public decimal Amount { get; set; }         // Valor
        public TransactionType Type { get; set; }   // Receita ou Despesa
        public string Category { get; set; } = "";  // Ex: Alimentação, Salário...
        public DateTime Date { get; set; } = DateTime.Now;

        public User? User { get; set; }
    }

    // Tipo de transação
    public enum TransactionType
    {
        Income,   // Receita
        Expense   // Despesa
    }

    // Meta de economia
    public class Goal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal TargetAmount { get; set; }   // Valor alvo
        public decimal CurrentAmount { get; set; }  // Valor atual economizado
        public DateTime Deadline { get; set; }      // Prazo
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User? User { get; set; }

        // Calcula a porcentagem de progresso
        public decimal ProgressPercent =>
            TargetAmount > 0 ? Math.Min(100, (CurrentAmount / TargetAmount) * 100) : 0;
    }

    // ==============================================
    // VIEW MODELS - Dados enviados para as Views (páginas HTML)
    // ==============================================

    public class LoginViewModel
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string? Error { get; set; }
    }

    public class RegisterViewModel
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string? Error { get; set; }
    }

    public class DashboardViewModel
    {
        public User User { get; set; } = new();
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> RecentTransactions { get; set; } = new();
        public List<Goal> Goals { get; set; } = new();
        public Dictionary<string, decimal> ExpensesByCategory { get; set; } = new();
        public List<MonthlyData> MonthlyData { get; set; } = new();
    }

    public class MonthlyData
    {
        public string Month { get; set; } = "";
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
    }

    public class TransactionViewModel
    {
        public string Description { get; set; } = "";
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string Category { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Now;
    }

    public class GoalViewModel
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime Deadline { get; set; } = DateTime.Now.AddMonths(6);
    }
}
