using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Models;

#nullable disable

namespace WebApplicationRestaurant.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishStatus> DishStatuses { get; set; }
        public DbSet<DishUnit> DishUnits { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientUnit> IngredientUnits { get; set; }
        public DbSet<MenuPlan> MenuPlans { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderIngredients> OrderIngredients { get; set; }
        public DbSet<OrderIngredientsItem> OrderIngredientsItem { get; set; }
        public DbSet<OrderIngredientsStatus> OrderIngredientsStatuses { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.ToTable("Dish");

                entity.HasIndex(e => e.CategoryId, "IX_Dish_fk_category");
                entity.HasIndex(e => e.DishUnitId, "IX_Dish_fk_unit");

                entity.Property(e => e.Cost)
                    .IsRequired()
                    .HasColumnName("cost");

                entity.Property(e => e.CategoryId)
                    .IsRequired()
                    .HasColumnName("fk_category");
                entity.Property(e => e.DishUnitId)
                    .IsRequired()
                    .HasColumnName("fk_unit");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name")
                    .IsFixedLength(true);

                entity.Property(e => e.Weight)
                    .IsRequired()
                    .HasColumnName("weight");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dish_Category");

                entity.HasOne(d => d.DishUnit)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.DishUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dish_Unit");

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
                            j.ToTable("ShoppingCartItem");

                            j.HasIndex(sc => sc.DishStatusId, "IX_SchoppingCartItem_fk_status");
                            j.HasIndex(sc => sc.UserId, "IX_SchoppingCartItem_fk_user");

                            j.HasKey(sc => new { sc.OrderId, sc.DishId });

                            j.Property(sc => sc.DishId).HasColumnName("fk_dish");
                            j.Property(sc => sc.OrderId).HasColumnName("fk_order");
                            j.Property(sc => sc.Count)
                                .HasColumnName("count")
                                .HasDefaultValue(1);
                            j.Property(sc => sc.DishStatusId)
                                .HasColumnName("fk_status")
                                .HasDefaultValue(1);
                            j.Property(sc => sc.UserId).HasColumnName("fk_user");

                            j.HasOne(sc => sc.DishStatus)
                                .WithMany(p => p.ShoppingCart)
                                .HasForeignKey(d => d.DishStatusId)
                                .OnDelete(DeleteBehavior.ClientSetNull)
                                .HasConstraintName("FK_SchoppingCartItem_DishStatus");

                            j.HasOne(sc => sc.User)
                                .WithMany(p => p.ShoppingCart)
                                .HasForeignKey(d => d.UserId)
                                .OnDelete(DeleteBehavior.ClientSetNull)
                                .HasConstraintName("FK_SchoppingCartItem_User");
                        });
            });

            modelBuilder.Entity<DishStatus>(entity =>
            {
                entity.ToTable("DishStatus");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name")
                    .IsFixedLength(true);

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasDefaultValue(0);
            });

            modelBuilder.Entity<DishUnit>(entity =>
            {
                entity.ToTable("DishUnit");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.ToTable("Ingredient");

                entity.HasIndex(e => e.IngredientUnitId, "IX_Ingredient_fk_unit");

                entity.Property(e => e.Count)
                    .IsRequired()
                    .HasColumnName("count");

                entity.Property(e => e.DeliveryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("delivery_date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name")
                    .IsFixedLength(true);

                entity.HasOne(d => d.IngredientUnit)
                    .WithMany(p => p.Ingredients)
                    .HasForeignKey(d => d.IngredientUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ingredient_Unit");

                entity.HasMany(i => i.Dishes)
                    .WithMany(d => d.Ingredients)
                    .UsingEntity(j => j.ToTable("Recipe"));
            });

            modelBuilder.Entity<IngredientUnit>(entity =>
            {
                entity.ToTable("IngredientUnit");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<MenuPlan>(entity =>
            {
                entity.ToTable("MenuPlan");

                entity.Property(e => e.PlanStartDate)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("plan_start_date");

                entity.Property(e => e.PlanEndDate)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("plan_end_date");

                entity.HasMany(i => i.Dishes)
                    .WithMany(d => d.MenuPlans)
                    .UsingEntity(j => j.ToTable("MenuPlanItem"));
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.HasIndex(e => e.StatusId, "IX_Order_fk_status");
                entity.HasIndex(e => e.PlaceId, "IX_Order_fk_place");
                entity.HasIndex(e => e.UserId, "IX_Order_fk_user");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasDefaultValue(0);

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("create_time")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.FinishTime)
                    .HasColumnType("datetime")
                    .HasColumnName("finish_time");

                entity.Property(e => e.StatusId)
                    .HasColumnName("fk_status")
                    .HasDefaultValue(1);

                entity.Property(e => e.PlaceId)
                    .IsRequired()
                    .HasColumnName("fk_place");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("fk_user");

                entity.HasOne(d => d.Status)
                        .WithMany(p => p.Orders)
                        .HasForeignKey(d => d.StatusId)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Order_OrderStatus");

                entity.HasOne(d => d.Place)
                        .WithMany(p => p.Orders)
                        .HasForeignKey(d => d.PlaceId)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Order_Place");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_User");
            });

            modelBuilder.Entity<OrderIngredients>(entity =>
            {
                entity.ToTable("OrderIngredients");

                entity.HasIndex(e => e.StatusId, "IX_OrderIngredients_fk_status");
                entity.HasIndex(e => e.UserId, "IX_OrderIngredients_fk_user");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.OrderDate)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("order_date");

                entity.Property(e => e.StatusId)
                    .HasColumnName("fk_status")
                    .HasDefaultValue(1);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("fk_user");

                entity.HasOne(d => d.Status)
                      .WithMany(p => p.OrderIngredients)
                      .HasForeignKey(d => d.StatusId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_OrderIngredients_OrderIngredientsStatus");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OrderIngredients)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderIngredients_User");

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
                            j.Property(sc => sc.Count)
                                .HasColumnName("count")
                                .HasDefaultValue(1);
                            j.HasKey(sc => new { sc.IngredientId, sc.OrderIngredientsId });
                            j.ToTable("OrderIngredientsItem");
                        });
            });

            modelBuilder.Entity<OrderIngredientsStatus>(entity =>
            {
                entity.ToTable("OrderIngredientsStatus");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name")
                    .IsFixedLength(true);

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasDefaultValue(0);
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("OrderStatus");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name")
                    .IsFixedLength(true);

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasDefaultValue(0);
            });

            modelBuilder.Entity<Place>(entity =>
            {
                entity.ToTable("Place");

                entity.Property(e => e.Number)
                    .HasColumnName("number");

                entity.Property(e => e.Available)
                    .HasColumnName("available")
                    .HasDefaultValue(1);

                entity.Property(e => e.ReserveTime)
                    .HasColumnType("datetime")
                    .HasColumnName("reserve_time");

                entity.Property(e => e.ReserverName)
                    .HasMaxLength(20)
                    .HasColumnName("reserver_name")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.HasIndex(e => e.UserId, "IX_OrderIngredients_fk_user");

                entity.Property(e => e.WorkingDate)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("working_date");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("fk_user");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Schedule_User");

                entity.HasMany(i => i.Places)
                    .WithMany(d => d.Schedules)
                    .UsingEntity(j => j.ToTable("SchedulePlace"));
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("Name")
                    .IsFixedLength(true);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("Surname")
                    .IsFixedLength(true);
            });
        }
    }
}
