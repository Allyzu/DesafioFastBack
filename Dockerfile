# 1. Imagem base do .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 2. Copia csproj e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# 3. Copia tudo e publica
COPY . ./
RUN dotnet publish -c Release -o out

# 4. Imagem runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# 5. Porta
EXPOSE 7084

# 6. Comando para rodar
ENTRYPOINT ["dotnet", "DesafioFast.dll"]
