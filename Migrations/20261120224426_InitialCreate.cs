using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApplicationRestaurant.Migrations;

/// <inheritdoc />
public partial class _20261120224426_InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "asp_net_roles",
            columns: table => new
            {
                id = table.Column<string>(type: "text", nullable: false),
                name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                concurrency_stamp = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_asp_net_roles", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "asp_net_users",
            columns: table => new
            {
                id = table.Column<string>(type: "text", nullable: false),
                name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                surname = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                password_hash = table.Column<string>(type: "text", nullable: true),
                security_stamp = table.Column<string>(type: "text", nullable: true),
                concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                phone_number = table.Column<string>(type: "text", nullable: true),
                phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                access_failed_count = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_asp_net_users", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "categories",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_categories", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "dish_statuses",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_dish_statuses", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "dish_units",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_dish_units", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "ingredient_units",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_ingredient_units", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "menu_plans",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                plan_start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                plan_end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_menu_plans", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "order_ingredients_statuses",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_order_ingredients_statuses", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "order_statuses",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_order_statuses", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "places",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                number = table.Column<int>(type: "integer", nullable: false),
                available = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                reserver_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                reserve_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_places", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "asp_net_role_claims",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                role_id = table.Column<string>(type: "text", nullable: false),
                claim_type = table.Column<string>(type: "text", nullable: true),
                claim_value = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_asp_net_role_claims", x => x.id);
                table.ForeignKey(
                    name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                    column: x => x.role_id,
                    principalTable: "asp_net_roles",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "asp_net_user_claims",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                user_id = table.Column<string>(type: "text", nullable: false),
                claim_type = table.Column<string>(type: "text", nullable: true),
                claim_value = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_asp_net_user_claims", x => x.id);
                table.ForeignKey(
                    name: "fk_asp_net_user_claims_asp_net_users_user_id",
                    column: x => x.user_id,
                    principalTable: "asp_net_users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "asp_net_user_logins",
            columns: table => new
            {
                login_provider = table.Column<string>(type: "text", nullable: false),
                provider_key = table.Column<string>(type: "text", nullable: false),
                provider_display_name = table.Column<string>(type: "text", nullable: true),
                user_id = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_asp_net_user_logins", x => new { x.login_provider, x.provider_key });
                table.ForeignKey(
                    name: "fk_asp_net_user_logins_asp_net_users_user_id",
                    column: x => x.user_id,
                    principalTable: "asp_net_users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "asp_net_user_roles",
            columns: table => new
            {
                user_id = table.Column<string>(type: "text", nullable: false),
                role_id = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_asp_net_user_roles", x => new { x.user_id, x.role_id });
                table.ForeignKey(
                    name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                    column: x => x.role_id,
                    principalTable: "asp_net_roles",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_asp_net_user_roles_asp_net_users_user_id",
                    column: x => x.user_id,
                    principalTable: "asp_net_users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "asp_net_user_tokens",
            columns: table => new
            {
                user_id = table.Column<string>(type: "text", nullable: false),
                login_provider = table.Column<string>(type: "text", nullable: false),
                name = table.Column<string>(type: "text", nullable: false),
                value = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_asp_net_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                table.ForeignKey(
                    name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                    column: x => x.user_id,
                    principalTable: "asp_net_users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "schedules",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                working_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                user_id = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_schedules", x => x.id);
                table.ForeignKey(
                    name: "fk_schedules_users_user_id",
                    column: x => x.user_id,
                    principalTable: "asp_net_users",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "dishes",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                weight = table.Column<double>(type: "double precision", nullable: false),
                cost = table.Column<double>(type: "double precision", nullable: false),
                category_id = table.Column<int>(type: "integer", nullable: false),
                dish_unit_id = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_dishes", x => x.id);
                table.ForeignKey(
                    name: "fk_dishes_categories_category_id",
                    column: x => x.category_id,
                    principalTable: "categories",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_dishes_dish_units_dish_unit_id",
                    column: x => x.dish_unit_id,
                    principalTable: "dish_units",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "ingredients",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                count = table.Column<double>(type: "double precision", nullable: false),
                delivery_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                ingredient_unit_id = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_ingredients", x => x.id);
                table.ForeignKey(
                    name: "fk_ingredients_ingredient_units_ingredient_unit_id",
                    column: x => x.ingredient_unit_id,
                    principalTable: "ingredient_units",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "order_ingredients",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                create_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "LOCALTIMESTAMP"),
                order_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                status_id = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                user_id = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_order_ingredients", x => x.id);
                table.ForeignKey(
                    name: "fk_order_ingredients_order_ingredients_statuses_status_id",
                    column: x => x.status_id,
                    principalTable: "order_ingredients_statuses",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_order_ingredients_users_user_id",
                    column: x => x.user_id,
                    principalTable: "asp_net_users",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "orders",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                create_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "LOCALTIMESTAMP"),
                finish_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                amount = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                status_id = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                place_id = table.Column<int>(type: "integer", nullable: false),
                user_id = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_orders", x => x.id);
                table.ForeignKey(
                    name: "fk_orders_order_statuses_status_id",
                    column: x => x.status_id,
                    principalTable: "order_statuses",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_orders_places_place_id",
                    column: x => x.place_id,
                    principalTable: "places",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_orders_users_user_id",
                    column: x => x.user_id,
                    principalTable: "asp_net_users",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "schedule_places",
            columns: table => new
            {
                places_id = table.Column<int>(type: "integer", nullable: false),
                schedules_id = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_schedule_places", x => new { x.places_id, x.schedules_id });
                table.ForeignKey(
                    name: "fk_schedule_places_places_places_id",
                    column: x => x.places_id,
                    principalTable: "places",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_schedule_places_schedules_schedules_id",
                    column: x => x.schedules_id,
                    principalTable: "schedules",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "menu_plan_items",
            columns: table => new
            {
                dishes_id = table.Column<int>(type: "integer", nullable: false),
                menu_plans_id = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_menu_plan_items", x => new { x.dishes_id, x.menu_plans_id });
                table.ForeignKey(
                    name: "fk_menu_plan_items_dishes_dishes_id",
                    column: x => x.dishes_id,
                    principalTable: "dishes",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_menu_plan_items_menu_plans_menu_plans_id",
                    column: x => x.menu_plans_id,
                    principalTable: "menu_plans",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "recipes",
            columns: table => new
            {
                dishes_id = table.Column<int>(type: "integer", nullable: false),
                ingredients_id = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_recipes", x => new { x.dishes_id, x.ingredients_id });
                table.ForeignKey(
                    name: "fk_recipes_dishes_dishes_id",
                    column: x => x.dishes_id,
                    principalTable: "dishes",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_recipes_ingredients_ingredients_id",
                    column: x => x.ingredients_id,
                    principalTable: "ingredients",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "order_ingredients_items",
            columns: table => new
            {
                ingredient_id = table.Column<int>(type: "integer", nullable: false),
                order_ingredients_id = table.Column<int>(type: "integer", nullable: false),
                count = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 1m)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_order_ingredients_items", x => new { x.ingredient_id, x.order_ingredients_id });
                table.ForeignKey(
                    name: "fk_order_ingredients_items_ingredients_ingredient_id",
                    column: x => x.ingredient_id,
                    principalTable: "ingredients",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_order_ingredients_items_order_ingredients_order_ingredients",
                    column: x => x.order_ingredients_id,
                    principalTable: "order_ingredients",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "shopping_cart_items",
            columns: table => new
            {
                dish_id = table.Column<int>(type: "integer", nullable: false),
                order_id = table.Column<int>(type: "integer", nullable: false),
                count = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                dish_status_id = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                user_id = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_shopping_cart_items", x => new { x.order_id, x.dish_id });
                table.ForeignKey(
                    name: "fk_shopping_cart_items_asp_net_users_user_id",
                    column: x => x.user_id,
                    principalTable: "asp_net_users",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_shopping_cart_items_dish_statuses_dish_status_id",
                    column: x => x.dish_status_id,
                    principalTable: "dish_statuses",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_shopping_cart_items_dishes_dish_id",
                    column: x => x.dish_id,
                    principalTable: "dishes",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_shopping_cart_items_orders_order_id",
                    column: x => x.order_id,
                    principalTable: "orders",
                    principalColumn: "id");
            });

        migrationBuilder.CreateIndex(
            name: "ix_asp_net_role_claims_role_id",
            table: "asp_net_role_claims",
            column: "role_id");

        migrationBuilder.CreateIndex(
            name: "role_name_index",
            table: "asp_net_roles",
            column: "normalized_name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_asp_net_user_claims_user_id",
            table: "asp_net_user_claims",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_asp_net_user_logins_user_id",
            table: "asp_net_user_logins",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_asp_net_user_roles_role_id",
            table: "asp_net_user_roles",
            column: "role_id");

        migrationBuilder.CreateIndex(
            name: "email_index",
            table: "asp_net_users",
            column: "normalized_email");

        migrationBuilder.CreateIndex(
            name: "user_name_index",
            table: "asp_net_users",
            column: "normalized_user_name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_dishes_category_id",
            table: "dishes",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "ix_dishes_dish_unit_id",
            table: "dishes",
            column: "dish_unit_id");

        migrationBuilder.CreateIndex(
            name: "ix_ingredients_ingredient_unit_id",
            table: "ingredients",
            column: "ingredient_unit_id");

        migrationBuilder.CreateIndex(
            name: "ix_menu_plan_items_menu_plans_id",
            table: "menu_plan_items",
            column: "menu_plans_id");

        migrationBuilder.CreateIndex(
            name: "ix_order_ingredients_status_id",
            table: "order_ingredients",
            column: "status_id");

        migrationBuilder.CreateIndex(
            name: "ix_order_ingredients_user_id",
            table: "order_ingredients",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_order_ingredients_items_order_ingredients_id",
            table: "order_ingredients_items",
            column: "order_ingredients_id");

        migrationBuilder.CreateIndex(
            name: "ix_orders_place_id",
            table: "orders",
            column: "place_id");

        migrationBuilder.CreateIndex(
            name: "ix_orders_status_id",
            table: "orders",
            column: "status_id");

        migrationBuilder.CreateIndex(
            name: "ix_orders_user_id",
            table: "orders",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_recipes_ingredients_id",
            table: "recipes",
            column: "ingredients_id");

        migrationBuilder.CreateIndex(
            name: "ix_schedule_places_schedules_id",
            table: "schedule_places",
            column: "schedules_id");

        migrationBuilder.CreateIndex(
            name: "ix_schedules_user_id",
            table: "schedules",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_shopping_cart_items_dish_id",
            table: "shopping_cart_items",
            column: "dish_id");

        migrationBuilder.CreateIndex(
            name: "ix_shopping_cart_items_dish_status_id",
            table: "shopping_cart_items",
            column: "dish_status_id");

        migrationBuilder.CreateIndex(
            name: "ix_shopping_cart_items_user_id",
            table: "shopping_cart_items",
            column: "user_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "asp_net_role_claims");

        migrationBuilder.DropTable(
            name: "asp_net_user_claims");

        migrationBuilder.DropTable(
            name: "asp_net_user_logins");

        migrationBuilder.DropTable(
            name: "asp_net_user_roles");

        migrationBuilder.DropTable(
            name: "asp_net_user_tokens");

        migrationBuilder.DropTable(
            name: "menu_plan_items");

        migrationBuilder.DropTable(
            name: "order_ingredients_items");

        migrationBuilder.DropTable(
            name: "recipes");

        migrationBuilder.DropTable(
            name: "schedule_places");

        migrationBuilder.DropTable(
            name: "shopping_cart_items");

        migrationBuilder.DropTable(
            name: "asp_net_roles");

        migrationBuilder.DropTable(
            name: "menu_plans");

        migrationBuilder.DropTable(
            name: "order_ingredients");

        migrationBuilder.DropTable(
            name: "ingredients");

        migrationBuilder.DropTable(
            name: "schedules");

        migrationBuilder.DropTable(
            name: "dish_statuses");

        migrationBuilder.DropTable(
            name: "dishes");

        migrationBuilder.DropTable(
            name: "orders");

        migrationBuilder.DropTable(
            name: "order_ingredients_statuses");

        migrationBuilder.DropTable(
            name: "ingredient_units");

        migrationBuilder.DropTable(
            name: "categories");

        migrationBuilder.DropTable(
            name: "dish_units");

        migrationBuilder.DropTable(
            name: "order_statuses");

        migrationBuilder.DropTable(
            name: "places");

        migrationBuilder.DropTable(
            name: "asp_net_users");
    }
}
