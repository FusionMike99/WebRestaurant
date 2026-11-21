using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<DishStatus> DishStatuses => Set<DishStatus>();
    public DbSet<DishUnit> DishUnits => Set<DishUnit>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<IngredientUnit> IngredientUnits => Set<IngredientUnit>();
    public DbSet<MenuPlan> MenuPlans => Set<MenuPlan>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderIngredients> OrderIngredients => Set<OrderIngredients>();
    public DbSet<OrderIngredientsItem> OrderIngredientsItem => Set<OrderIngredientsItem>();
    public DbSet<OrderIngredientsStatus> OrderIngredientsStatuses => Set<OrderIngredientsStatus>();
    public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
    public DbSet<Place> Places => Set<Place>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<ShoppingCartItem> ShoppingCartItem => Set<ShoppingCartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // IdentityDbContext explicitly configures PascalCase table and index names,
        // which take precedence over EFCore.NamingConventions; map them to snake_case explicitly.
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("asp_net_users");
            entity.HasIndex(u => u.NormalizedEmail).HasDatabaseName("email_index");
            entity.HasIndex(u => u.NormalizedUserName).HasDatabaseName("user_name_index");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Surname)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable("asp_net_roles");
            entity.HasIndex(r => r.NormalizedName).HasDatabaseName("role_name_index");
        });

        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("asp_net_user_roles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("asp_net_user_claims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("asp_net_user_logins");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("asp_net_user_tokens");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("asp_net_role_claims");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Dishes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.DishUnit)
                .WithMany(p => p.Dishes)
                .HasForeignKey(d => d.DishUnitId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(d => d.Orders)
                .WithMany(o => o.Dishes)
                .UsingEntity<ShoppingCartItem>(
                    j => j
                        .HasOne(sc => sc.Order)
                        .WithMany(d => d.ShoppingCart)
                        .HasForeignKey(sc => sc.OrderId)
                        .OnDelete(DeleteBehavior.ClientCascade),
                    j => j
                        .HasOne(sc => sc.Dish)
                        .WithMany(o => o.ShoppingCart)
                        .HasForeignKey(sc => sc.DishId)
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.ToTable("shopping_cart_items");

                        j.HasKey(sc => new { sc.OrderId, sc.DishId });

                        j.Property(sc => sc.Count)
                            .HasDefaultValue(1);

                        j.Property(sc => sc.DishStatusId)
                            .HasDefaultValue(DishStatus.AwaitingId);

                        j.HasOne(sc => sc.DishStatus)
                            .WithMany(p => p.ShoppingCart)
                            .HasForeignKey(d => d.DishStatusId)
                            .OnDelete(DeleteBehavior.ClientSetNull);

                        j.HasOne(sc => sc.User)
                            .WithMany(p => p.ShoppingCart)
                            .HasForeignKey(d => d.UserId)
                            .OnDelete(DeleteBehavior.ClientSetNull);
                    });
        });

        modelBuilder.Entity<DishStatus>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Count)
                .HasDefaultValue(0);
        });

        modelBuilder.Entity<DishUnit>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.Property(e => e.DeliveryDate)
                .HasColumnType("timestamp without time zone");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasOne(d => d.IngredientUnit)
                .WithMany(p => p.Ingredients)
                .HasForeignKey(d => d.IngredientUnitId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(i => i.Dishes)
                .WithMany(d => d.Ingredients)
                .UsingEntity(j => j.ToTable("recipes"));
        });

        modelBuilder.Entity<IngredientUnit>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<MenuPlan>(entity =>
        {
            entity.Property(e => e.PlanStartDate)
                .HasColumnType("timestamp without time zone");

            entity.Property(e => e.PlanEndDate)
                .HasColumnType("timestamp without time zone");

            entity.HasMany(i => i.Dishes)
                .WithMany(d => d.MenuPlans)
                .UsingEntity(j => j.ToTable("menu_plan_items"));
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Amount)
                .HasDefaultValue(0.0);

            entity.Property(e => e.CreateTime)
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("LOCALTIMESTAMP");

            entity.Property(e => e.FinishTime)
                .HasColumnType("timestamp without time zone");

            entity.Property(e => e.StatusId)
                .HasDefaultValue(OrderStatus.OpenId);

            entity.Property(e => e.UserId)
                .IsRequired();

            entity.HasOne(d => d.Status)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Place)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.PlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<OrderIngredients>(entity =>
        {
            entity.Property(e => e.CreateDate)
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("LOCALTIMESTAMP");

            entity.Property(e => e.OrderDate)
                .HasColumnType("timestamp without time zone");

            entity.Property(e => e.StatusId)
                .HasDefaultValue(OrderIngredientsStatus.OpenId);

            entity.Property(e => e.UserId)
                .IsRequired();

            entity.HasOne(d => d.Status)
                .WithMany(p => p.OrderIngredients)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User)
                .WithMany(p => p.OrderIngredients)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(d => d.Ingredients)
                .WithMany(o => o.Orders)
                .UsingEntity<OrderIngredientsItem>(
                    j => j
                        .HasOne(sc => sc.Ingredient)
                        .WithMany(d => d.OrderIngredientsItems)
                        .HasForeignKey(sc => sc.IngredientId)
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j => j
                        .HasOne(sc => sc.OrderIngredients)
                        .WithMany(o => o.OrderIngredientsItems)
                        .HasForeignKey(sc => sc.OrderIngredientsId)
                        .OnDelete(DeleteBehavior.ClientCascade),
                    j =>
                    {
                        j.ToTable("order_ingredients_items");
                        j.HasKey(sc => new { sc.IngredientId, sc.OrderIngredientsId });
                        j.Property(sc => sc.Count)
                            .HasDefaultValue(1m);
                    });
        });

        modelBuilder.Entity<OrderIngredientsStatus>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Count)
                .HasDefaultValue(0);
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Count)
                .HasDefaultValue(0);
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.Property(e => e.Available)
                .HasDefaultValue(true);

            entity.Property(e => e.ReserveTime)
                .HasColumnType("timestamp without time zone");

            entity.Property(e => e.ReserverName)
                .HasMaxLength(20);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.Property(e => e.WorkingDate)
                .HasColumnType("timestamp without time zone");

            entity.Property(e => e.UserId)
                .IsRequired();

            entity.HasOne(d => d.User)
                .WithMany(p => p.Schedules)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(i => i.Places)
                .WithMany(d => d.Schedules)
                .UsingEntity(j => j.ToTable("schedule_places"));
        });
    }
}
