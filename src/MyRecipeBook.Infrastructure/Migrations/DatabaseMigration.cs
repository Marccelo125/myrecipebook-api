using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace MyRecipeBook.Infrastructure.Migrations
{
    public static class DatabaseMigration
    {
        public static void Migrate(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            // Manipulando a string de conexão:
            var connectionString = configuration.GetConnectionString("ConnectionMySql");
            
            // Vai ver se a conexão existe de verdade
            var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString); 
            
            // Pega o nome do banco de dados que esta na string de conexao (no arquivo da api)
            var databaseName = connectionStringBuilder.Database;

            // Tira o banco de dados da string de conexão para nao dar erro na migration
            connectionStringBuilder.Remove("Database");

            // Cria uma conexão com o banco de dados com a nova string de conexão
            using var dbConnection = new MySqlConnection(connectionStringBuilder.ConnectionString);
            
            // Criando parameter do Dapper para passar na query
            var parameters = new DynamicParameters();
            parameters.Add("databaseName", databaseName);
            
            // Consultando a tabela de Schema para ver se o database já existe
            var records = dbConnection.Query("SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @databaseName", parameters);
            
            // Se o banco não existe, cria um novo
            if (!records.Any())
                dbConnection.Execute($"CREATE DATABASE {databaseName}");

            // Faz a migração da database
            MigrationDatabase(serviceProvider);
        }

        private static void MigrationDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            
            runner.ListMigrations();
            runner.MigrateUp();
        }
    }
}
