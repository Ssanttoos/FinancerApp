FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY FinanceiroApp.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
RUN mkdir -p /data
ENV ConnectionStrings__Default="Data Source=/data/financeiro.db"
EXPOSE 8080
ENTRYPOINT ["dotnet", "FinanceiroApp.dll"]
