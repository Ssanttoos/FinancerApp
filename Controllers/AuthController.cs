using Microsoft.AspNetCore.Mvc;
using FinanceiroApp.Data;
using FinanceiroApp.Models;

namespace FinanceiroApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _db;

        // Injeção de dependência: o ASP.NET nos dá o banco de dados automaticamente
        public AuthController(AppDbContext db)
        {
            _db = db;
        }

        // GET /Auth/Login - Exibe a tela de login
        public IActionResult Login()
        {
            // Se já está logado, vai direto para o dashboard
            if (HttpContext.Session.GetInt32("UserId") != null)
                return RedirectToAction("Index", "Home");

            return View(new LoginViewModel());
        }

        // POST /Auth/Login - Processa o formulário de login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            // Busca o usuário pelo email
            var user = _db.Users.FirstOrDefault(u => u.Email == model.Email);

            // Verifica se o usuário existe e a senha está correta
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                model.Error = "Email ou senha incorretos.";
                return View(model);
            }

            // Salva o ID do usuário na sessão (mantém o login)
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Home");
        }

        // GET /Auth/Register - Exibe a tela de cadastro
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        // POST /Auth/Register - Processa o cadastro
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            // Verifica se o email já está em uso
            if (_db.Users.Any(u => u.Email == model.Email))
            {
                model.Error = "Este email já está cadastrado.";
                return View(model);
            }

            // Cria o novo usuário com senha criptografada
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password) // NUNCA salve senha em texto puro!
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            // Loga automaticamente após o cadastro
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Home");
        }

        // GET /Auth/Logout - Faz logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
