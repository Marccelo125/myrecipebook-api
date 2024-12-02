using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(2, "Create table to save users Recipes")]
    public class Version0000002 : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("Recipes")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("CreationUtcDate").AsDateTime().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable()
            .WithColumn("Title").AsString(255).NotNullable()
            .WithColumn("CookingTime").AsInt32().Nullable()
            .WithColumn("Difficulty").AsInt32().Nullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Recipes_Reference_User", "Users", "Id");

            Create.Index("IX_Recipes_UserId").OnTable("Recipes").OnColumn("UserId").Ascending();

            Create.Table("Ingredients")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("CreationUtcDate").AsDateTime().NotNullable()
            .WithColumn("Item").AsString(255).NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Ingredients_Reference_Recipe", "Recipes", "Id")
            .OnDelete(System.Data.Rule.Cascade);

            Create.Index("IX_Ingredients_RecipeId").OnTable("Ingredients").OnColumn("RecipeId").Ascending();

            Create.Table("Instructions")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("CreationUtcDate").AsDateTime().NotNullable()
            .WithColumn("Step").AsInt32().NotNullable()
            .WithColumn("Text").AsString(2000)
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Instructions_Reference_Recipe", "Recipes", "Id")
            .OnDelete(System.Data.Rule.Cascade);
            
            Create.Index("IX_Instructions_RecipeId").OnTable("Instructions").OnColumn("RecipeId").Ascending();

            Create.Table("DishTypes")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("CreationUtcDate").AsDateTime().NotNullable()
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_DishTypes_Reference_Recipe", "Recipes", "Id")
            .OnDelete(System.Data.Rule.Cascade);
            
            Create.Index("IX_DishTypes_RecipeId").OnTable("DishTypes").OnColumn("RecipeId").Ascending();
        }
    }
}
