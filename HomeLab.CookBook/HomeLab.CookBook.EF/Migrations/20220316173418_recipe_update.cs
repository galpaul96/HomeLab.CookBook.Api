using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeLab.API.CookBook.EF.Migrations
{
    public partial class recipe_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredient_Step_StepId",
                table: "Ingredient");

            migrationBuilder.DropForeignKey(
                name: "FK_Step_Recipes_RecipeId",
                table: "Step");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Step",
                table: "Step");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient");

            migrationBuilder.RenameTable(
                name: "Step",
                newName: "Steps");

            migrationBuilder.RenameTable(
                name: "Ingredient",
                newName: "Ingredients");

            migrationBuilder.RenameIndex(
                name: "IX_Step_RecipeId",
                table: "Steps",
                newName: "IX_Steps_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Step_Id",
                table: "Steps",
                newName: "IX_Steps_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredient_StepId",
                table: "Ingredients",
                newName: "IX_Ingredients_StepId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredient_Id",
                table: "Ingredients",
                newName: "IX_Ingredients_Id");

            migrationBuilder.AddColumn<string>(
                name: "Difficulty",
                table: "Recipes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Steps",
                table: "Steps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Steps_StepId",
                table: "Ingredients",
                column: "StepId",
                principalTable: "Steps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_Recipes_RecipeId",
                table: "Steps",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Steps_StepId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Steps_Recipes_RecipeId",
                table: "Steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Steps",
                table: "Steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Recipes");

            migrationBuilder.RenameTable(
                name: "Steps",
                newName: "Step");

            migrationBuilder.RenameTable(
                name: "Ingredients",
                newName: "Ingredient");

            migrationBuilder.RenameIndex(
                name: "IX_Steps_RecipeId",
                table: "Step",
                newName: "IX_Step_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Steps_Id",
                table: "Step",
                newName: "IX_Step_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_StepId",
                table: "Ingredient",
                newName: "IX_Ingredient_StepId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_Id",
                table: "Ingredient",
                newName: "IX_Ingredient_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Step",
                table: "Step",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_Step_StepId",
                table: "Ingredient",
                column: "StepId",
                principalTable: "Step",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Step_Recipes_RecipeId",
                table: "Step",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
