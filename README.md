# 📙[My Recipe Book API](https://github.com/Marccelo125/myrecipebook-api) 📦

> [!IMPORTANT]
> Este é um projeto de estudo e aplicação de uma API em .NET 8. O foco é o desenvolvimento de um sistema de receitas.</br>

> [!NOTE]
> O desenvolvimento deste projeto é uma base para estudo de DDD em .NET 8. Aprendi muito com ele então caso queira dar um `Fork`, sinta-se a vontade para contribuir.

# Instalação .NET

Para instalar o .NET 8, siga as instruções no [guia de instalação oficial]([https://laravel.com/docs/installation](https://learn.microsoft.com/pt-br/dotnet/core/install/)).

Certifique-se de que possui o SDK do .NET 8 instalado e um editor de texto compatível, como o Visual Studio, Visual Studio Code ou JetBrains Rider.

Comandos iniciais para criação e execução de um projeto:

```
dotnet new webapi -n MeuProjeto # Cria um novo projeto Web API
cd MeuProjeto
dotnet run # Executa o projeto
```

# Configuração

A configuração do .NET 8 é feita através do arquivo `appsettings.json` ou variáveis de ambiente. Para conectar-se a um banco de dados local, você pode ajustar a string de conexão no `appsettings.json`.

## Exemplo de configuração local:
### appsettings.json:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Library;User Id=sa;Password=SuaSenha;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
Para rodar um banco de dados local, você pode utilizar ferramentas como Docker, SQL Server LocalDB ou o Azure Data Studio.
