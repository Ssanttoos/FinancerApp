# ====================================================
# DOCKERFILE — Instruções para empacotar a aplicação
# O Railway usa esse arquivo para rodar o projeto
# ====================================================

# ETAPA 1: Build — compila o projeto C#
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos do projeto
COPY FinanceiroApp.csproj .
RUN dotnet restore

# Copia o restante e publica em modo Release
COPY . .
RUN dotnet publish -c Release -o /app/publish

# ====================================================
# ETAPA 2: Runtime — imagem final (muito mais leve)
# ====================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia apenas o resultado do build
COPY --from=build /app/publish .

# Garante que o banco SQLite seja salvo numa pasta persistente
RUN mkdir -p /data
ENV ConnectionStrings__Default="Data Source=/data/financeiro.db"

# Railway define a porta via variável de ambiente
ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE 8080

ENTRYPOINT ["dotnet", "FinanceiroApp.dll"]
