# 💰 Financer — Assistente Financeiro Pessoal

Sistema web de controle financeiro desenvolvido em **C# / ASP.NET Core 8** com banco de dados **SQLite**.

---

## 🚀 Como rodar o projeto

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado

### Passo a passo

**1. Restaurar as dependências (baixa os pacotes NuGet)**
```bash
dotnet restore
```

**2. Rodar o projeto**
```bash
dotnet run
```

**3. Abrir no navegador**
```
http://localhost:5000
```

> O banco de dados SQLite (`financeiro.db`) é criado **automaticamente** na primeira execução. Não precisa instalar nada!

---

## 📁 Estrutura do projeto

```
FinanceiroApp/
├── Controllers/          ← Lógica de negócio (C#)
│   ├── AuthController.cs    → Login e cadastro
│   ├── HomeController.cs    → Dashboard
│   ├── TransactionsController.cs → Transações
│   └── GoalsController.cs   → Metas de economia
│
├── Models/               ← Estrutura dos dados
│   └── Models.cs            → User, Transaction, Goal...
│
├── Data/                 ← Banco de dados
│   └── AppDbContext.cs      → Configuração do EF Core
│
├── Views/                ← Páginas HTML (Razor)
│   ├── Auth/                → Login e Cadastro
│   ├── Home/                → Dashboard com gráficos
│   ├── Transactions/        → Lista de transações
│   ├── Goals/               → Metas de economia
│   └── Shared/              → Layout e componentes compartilhados
│
├── wwwroot/              ← Arquivos estáticos
│   ├── css/site.css         → Estilos (design dark)
│   └── js/site.js           → JavaScript (modais)
│
└── Program.cs            ← Configuração principal da aplicação
```

---

## ✅ Funcionalidades

- **Sistema de login/cadastro** com senha criptografada (BCrypt)
- **Dashboard** com resumo do mês, gráfico de barras e gráfico de pizza
- **Transações** — cadastro, listagem com filtros e exclusão de receitas/despesas
- **Metas de economia** — criação, acompanhamento de progresso e adição de valores
- **Design dark moderno** responsivo

---

## 🛠️ Tecnologias usadas

| Tecnologia | Para quê? |
|---|---|
| ASP.NET Core 8 (MVC) | Framework web em C# |
| Entity Framework Core | ORM — abstração do banco de dados |
| SQLite | Banco de dados leve, sem instalação |
| BCrypt.Net | Criptografia de senhas |
| Chart.js | Gráficos interativos |
| Google Fonts | Tipografia (Syne + DM Sans) |

---

## 💡 Próximos passos sugeridos

1. **Exportar relatório em PDF/Excel**
2. **Notificações de metas próximas do prazo**
3. **Gráfico de evolução patrimonial**
4. **Suporte a múltiplas moedas**
5. **Deploy no Azure ou Railway**
