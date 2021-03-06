using System;
using DigiStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DigiStore.Data.Context
{
    public partial class DigiStore_DBContext : DbContext
    {
        public DigiStore_DBContext()
        {
        }

        public DigiStore_DBContext(DbContextOptions<DigiStore_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<ContactU> ContactUs { get; set; }
        public virtual DbSet<FavoriteProductUser> FavoriteProductUsers { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public virtual DbSet<ProductDiscountUse> ProductDiscountUses { get; set; }
        public virtual DbSet<ProductFeature> ProductFeatures { get; set; }
        public virtual DbSet<ProductGallery> ProductGalleries { get; set; }
        public virtual DbSet<ProductRating> ProductRatings { get; set; }
        public virtual DbSet<ProductSelectedCategory> ProductSelectedCategories { get; set; }
        public virtual DbSet<ProductVisited> ProductVisiteds { get; set; }
        public virtual DbSet<Productcomment> Productcomments { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<SalesInforamtion> SalesInforamtions { get; set; }
        public virtual DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }
        public virtual DbSet<SalesOrderHeader> SalesOrderHeaders { get; set; }
        public virtual DbSet<Seller> Sellers { get; set; }
        public virtual DbSet<SellerWallet> SellerWallets { get; set; }
        public virtual DbSet<SiteSetting> SiteSettings { get; set; }
        public virtual DbSet<Slider> Sliders { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketMessage> TicketMessages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQL2019;Database=DigiStore_DB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Arabic_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address", "Users");

                entity.HasIndex(e => e.IsDelete, "I_NC_Users_Address_IsDelete");

                entity.HasIndex(e => e.Zipcode, "I_NC_Users_Address_Zipcode");

                entity.HasIndex(e => e.PostalCode, "I_NC_Users_Address_postalcard");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(800)
                    .HasColumnName("Address");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.Unit)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Zipcode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Address_CityID");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Address_StateID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Address_UserID");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand", "Brands");

                entity.Property(e => e.BrandName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Logo).HasMaxLength(200);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City", "Addresses");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Addresses_City_StateID");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.ToTable("Color", "Production");

                entity.HasIndex(e => e.ColorCode, "I_NC_Production_Color_ColorCode");

                entity.HasIndex(e => e.Price, "I_NC_Production_Color_Price");

                entity.Property(e => e.ColorCode)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Colors)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Production_Color_ProductId");
            });

            modelBuilder.Entity<ContactU>(entity =>
            {
                entity.HasKey(e => e.ContactUsid)
                    .HasName("PK_Site_ContactUs_ContactUs");

                entity.ToTable("ContactUs", "Site");

                entity.HasIndex(e => e.Email, "I_NC_Stie_ContactUs_Email");

                entity.HasIndex(e => e.FullName, "I_NC_Stie_ContactUs_FullName");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Text).IsRequired();

                entity.Property(e => e.UserIp)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FavoriteProductUser>(entity =>
            {
                entity.ToTable("FavoriteProductUser", "Production");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FavoriteProductUsers)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Production_FavoriteProductUser_ProductId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteProductUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Production_FavoriteProductUser_UserId");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permission", "Users");

                entity.HasIndex(e => e.PermissionTitle, "I_NC_Users_Permission_PermissionTitle");

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.PermissionTitle)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Users_Permission_ParentID");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "Production");

                entity.HasIndex(e => e.IsActive, "I_NC_Production_Product_IsActive");

                entity.HasIndex(e => e.IsDelete, "I_NC_Production_Product_IsDelete");

                entity.HasIndex(e => e.Name, "I_NC_Production_Product_Name");

                entity.HasIndex(e => e.Price, "I_NC_Production_Product_Price");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ImageName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ShortDescription)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.SiteProfile).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Production_Product_BrandId");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Production_Product_SellerId");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory", "Production");

                entity.HasIndex(e => e.IsActive, "I_NC_Production_ProductCategory_IsActive");

                entity.HasIndex(e => e.IsDelete, "I_NC_Production_ProductCategory_IsDelete");

                entity.HasIndex(e => e.Title, "I_NC_Production_ProductCategory_Title");

                entity.Property(e => e.IconName).HasMaxLength(200);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UrlName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Production_ProductCategory_ParentId");
            });

            modelBuilder.Entity<ProductDiscount>(entity =>
            {
                entity.ToTable("ProductDiscount", "Discount");

                entity.Property(e => e.ExpierDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductDiscounts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Discount_ProductDiscount_ProductId");
            });

            modelBuilder.Entity<ProductDiscountUse>(entity =>
            {
                entity.ToTable("ProductDiscountUse", "Discount");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.ProductDiscount)
                    .WithMany(p => p.ProductDiscountUses)
                    .HasForeignKey(d => d.ProductDiscountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Discount_ProductDiscountUse_ProductDiscountId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProductDiscountUses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Discount_ProductDiscountUse_UserID");
            });

            modelBuilder.Entity<ProductFeature>(entity =>
            {
                entity.ToTable("ProductFeature", "Production");

                entity.Property(e => e.FeatureTitle)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.FeatureValue)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductFeatures)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Production_ProductFeature_ProductId");
            });

            modelBuilder.Entity<ProductGallery>(entity =>
            {
                entity.ToTable("ProductGallery", "Production");

                entity.Property(e => e.ImageName)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductGalleries)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Production_ProductGallery_ProductId");
            });

            modelBuilder.Entity<ProductRating>(entity =>
            {
                entity.ToTable("ProductRating", "Production");

                entity.Property(e => e.ConstructionQuality).HasDefaultValueSql("((1))");

                entity.Property(e => e.DesignAndAppearance).HasDefaultValueSql("((1))");

                entity.Property(e => e.EaseOfUse).HasDefaultValueSql("((1))");

                entity.Property(e => e.FeaturesAndCapabilities).HasDefaultValueSql("((1))");

                entity.Property(e => e.Innovation).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PurchaseValueRelativeToPrice).HasDefaultValueSql("((1))");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductRatings)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Production_ProductRating_ProductId");
            });

            modelBuilder.Entity<ProductSelectedCategory>(entity =>
            {
                entity.ToTable("ProductSelectedCategory", "Production");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.ProductSelectedCategories)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK_Production_ProductSelectedCategory_ProductCategoryId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductSelectedCategories)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Production_ProductSelectedCategory_ProductId");
            });

            modelBuilder.Entity<ProductVisited>(entity =>
            {
                entity.HasKey(e => e.VisiteId)
                    .HasName("PK_Production_ProductVisited_VisiteId");

                entity.ToTable("ProductVisited", "Production");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.UserIp)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductVisiteds)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Production_ProductVisited_ProductId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProductVisiteds)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Production_ProductVisited_UserID");
            });

            modelBuilder.Entity<Productcomment>(entity =>
            {
                entity.ToTable("Productcomment", "Production");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(900);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Productcomments)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Production_Productcomment_ProductId");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.Productcomments)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Production_Productcomment_SellerId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Productcomments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Production_Productcomment_UserId");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles", "Users");

                entity.HasIndex(e => e.IsDelete, "I_NC_Users_Roles_IsDelete");

                entity.HasIndex(e => e.RoleTitle, "I_NC_Users_Roles_RoleTitle");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RoleTitle)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("RolePermission", "Users");

                entity.Property(e => e.RolePermissionId).HasColumnName("RolePermissionID");

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_RolePermission_PermissionID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_RolePermission_RoleID");
            });

            modelBuilder.Entity<SalesInforamtion>(entity =>
            {
                entity.ToTable("SalesInforamtion", "Sales");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReceiverMobile)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ReceiverNaationalId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ReceiverName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.SalesInforamtions)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sales_SalesInforamtion_AddressId");

                entity.HasOne(d => d.SalesOrder)
                    .WithMany(p => p.SalesInforamtions)
                    .HasForeignKey(d => d.SalesOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sales_SalesInforamtion_SalesOrderId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SalesInforamtions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sales_SalesInforamtion_UserID");
            });

            modelBuilder.Entity<SalesOrderDetail>(entity =>
            {
                entity.ToTable("SalesOrderDetail", "Sales");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.SalesOrderDetails)
                    .HasForeignKey(d => d.ColorId)
                    .HasConstraintName("FK_Sales_SalesOrderDetail_ColorId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SalesOrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sales_SalesOrderDetail_ProductId");

                entity.HasOne(d => d.SalesOrder)
                    .WithMany(p => p.SalesOrderDetails)
                    .HasForeignKey(d => d.SalesOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sales_SalesOrderDetail_SalesOrderId");
            });

            modelBuilder.Entity<SalesOrderHeader>(entity =>
            {
                entity.HasKey(e => e.SalesOrderId)
                    .HasName("PK_Sales_SalesOrderHeader_SalesOrderId");

                entity.ToTable("SalesOrderHeader", "Sales");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Descriptions).HasMaxLength(500);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.TracingCode).HasMaxLength(300);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SalesOrderHeaders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sales_SalesOrderHeader_UserID");
            });

            modelBuilder.Entity<Seller>(entity =>
            {
                entity.ToTable("Seller", "Store");

                entity.HasIndex(e => e.IsDelete, "I_NC_Store_Seller_IsDelete");

                entity.HasIndex(e => e.Logo, "I_NC_Store_Seller_Logo");

                entity.HasIndex(e => e.Phone, "I_NC_Store_Seller_Phone");

                entity.HasIndex(e => e.StoreName, "I_NC_Store_Seller_StoreName");

                entity.HasIndex(e => e.StoreaceptanceState, "I_NC_Store_Seller_StoreaceptanceState");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(700);

                entity.Property(e => e.AdminDescription).HasMaxLength(700);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Descriptions)
                    .IsRequired()
                    .HasMaxLength(700);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Logo).HasMaxLength(200);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.StoreAceptanceStateDescription).HasMaxLength(500);

                entity.Property(e => e.StoreDoucument).HasMaxLength(200);

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sellers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Store_Seller_UserID");
            });

            modelBuilder.Entity<SellerWallet>(entity =>
            {
                entity.ToTable("SellerWallet", "Store");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Descriptions).HasMaxLength(500);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.SellerWallets)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Store_SellerWallet_SellerId");
            });

            modelBuilder.Entity<SiteSetting>(entity =>
            {
                entity.ToTable("SiteSetting", "Site");

                entity.Property(e => e.SiteSettingId).HasColumnName("SiteSettingID");

                entity.Property(e => e.AboutUs).HasColumnName("AboutUS");

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Phone).HasMaxLength(200);

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<Slider>(entity =>
            {
                entity.ToTable("Slider", "Site");

                entity.HasIndex(e => e.ImageName, "I_NC_Site_Slider_ImageName");

                entity.HasIndex(e => e.Link, "I_NC_Site_Slider_Link");

                entity.Property(e => e.SliderId).HasColumnName("SliderID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ImageName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("State", "Addresses");

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.StateName)
                    .IsRequired()
                    .HasMaxLength(300);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket", "Ticket");

                entity.Property(e => e.TicketId).HasColumnName("TicketID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Ticket_Ticket_UserID");
            });

            modelBuilder.Entity<TicketMessage>(entity =>
            {
                entity.ToTable("TicketMessage", "Ticket");

                entity.Property(e => e.TicketMessageId).HasColumnName("TicketMessageID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Text).IsRequired();

                entity.Property(e => e.TicketId).HasColumnName("TicketID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.TicketMessages)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("FK_Ticket_TicketMessage_TicketID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TicketMessages)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Ticket_TicketMessage_UserID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "Users");

                entity.HasIndex(e => e.Email, "I_NC_Users_User_Email");

                entity.HasIndex(e => e.Mobile, "I_NC_Users_User_Mobile");

                entity.HasIndex(e => e.PassWord, "I_NC_Users_User_PassWord");

                entity.HasIndex(e => e.UserName, "I_NC_Users_User_UserName");

                entity.HasIndex(e => e.Email, "UQ_Users_User_Email")
                    .IsUnique();

                entity.HasIndex(e => e.UserName, "UQ_Users_User_UserName")
                    .IsUnique();

                entity.HasIndex(e => e.Mobile, "UQ__User__6FAE07822DAE8834")
                    .IsUnique();

                entity.HasIndex(e => e.Mobile, "UQ__User__6FAE0782AD1F72EE")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.ActiveCode).HasMaxLength(50);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.FirstName).HasMaxLength(200);

                entity.Property(e => e.FullName)
                    .HasMaxLength(401)
                    .HasComputedColumnSql("(([FirstName]+' ')+[LastName])", true);

                entity.Property(e => e.LastName).HasMaxLength(200);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PassWord)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.UserAvatar).HasMaxLength(200);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles", "Users");

                entity.HasIndex(e => e.RoleId, "I_NC_Users_UserRoles_RoleID");

                entity.HasIndex(e => e.UserId, "I_NC_Users_UserRoles_UserID");

                entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_UserRoles_RoleID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_UserRoles_UserID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
