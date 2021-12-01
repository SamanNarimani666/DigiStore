using System;
using DigiStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DigiStore.Context.Entities
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

        public virtual DbSet<ContactU> ContactUs { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<SiteSetting> SiteSettings { get; set; }
        public virtual DbSet<Slider> Sliders { get; set; }
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

                entity.HasIndex(e => e.Mobile, "UQ__User__6FAE0782FD948D88")
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
