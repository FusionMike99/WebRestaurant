using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationRestaurant.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "number",
                table: "Place",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "count",
                table: "OrderIngredientsItem",
                type: "float",
                nullable: false,
                defaultValue: 1.0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { 1, "First meals" },
                    { 2, "Second meals" },
                    { 3, "Salads" },
                    { 4, "Pizzas" },
                    { 5, "Drinks" }
                });

            migrationBuilder.InsertData(
                table: "DishStatus",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { 1, "awaiting" },
                    { 2, "accepted" },
                    { 3, "ready" }
                });

            migrationBuilder.InsertData(
                table: "DishUnit",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { 3, "літр" },
                    { 1, "грам" },
                    { 2, "кілограм" }
                });

            migrationBuilder.InsertData(
                table: "IngredientUnit",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { 1, "грам" },
                    { 2, "кілограм" },
                    { 3, "літр" }
                });

            migrationBuilder.InsertData(
                table: "OrderIngredientsStatus",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { 1, "open" },
                    { 2, "close" }
                });

            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { 1, "open" },
                    { 2, "close" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "DishStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DishStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DishStatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DishUnit",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DishUnit",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DishUnit",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "IngredientUnit",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "IngredientUnit",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "IngredientUnit",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderIngredientsStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderIngredientsStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "number",
                table: "Place");

            migrationBuilder.AlterColumn<int>(
                name: "count",
                table: "OrderIngredientsItem",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 1.0);
        }
    }
}
