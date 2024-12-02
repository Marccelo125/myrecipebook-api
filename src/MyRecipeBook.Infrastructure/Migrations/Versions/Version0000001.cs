using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(1, "Create table to save users information")]
    public class Version0000001 : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("Users")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("CreationUtcDate").AsDateTime().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable()
            .WithColumn("BirthDate").AsDate().NotNullable()
            .WithColumn("UserIdentifier").AsGuid().NotNullable();
        }
    }
}
