using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EA.Core.Domain.Models;

namespace EA.Core.Infra.Mappings
{    
    public class PermissionMap : IEntityTypeConfiguration<Permissions>
    {
        public void Configure(EntityTypeBuilder<Permissions> entity)
        {
             
                entity.ToTable("PERMISSIONS", "core");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
           
        }
    }
    public class PrivilegeMap : IEntityTypeConfiguration<Privileges>
    {
        public void Configure(EntityTypeBuilder<Privileges> entity)
        {

            entity.ToTable("PRIVILEGES", "core");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Permission)
                .WithMany(p => p.Privileges)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRIVILEGES_PERMISSIONS");

            entity.HasOne(d => d.Resource)
                .WithMany(p => p.Privileges)
                .HasForeignKey(d => d.ResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRIVILEGES_RESOURCES");
        }
    }


    public class ResAttributeMap : IEntityTypeConfiguration<ResAttributes>
    {
        public void Configure(EntityTypeBuilder<ResAttributes> entity)
        {
            entity.ToTable("RES_ATTRIBUTES", "core");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Key)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Value).HasMaxLength(255);

            entity.HasOne(d => d.Resource)
                .WithMany(p => p.ResAttributes)
                .HasForeignKey(d => d.ResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RES_ATTRIBUTES_RESOURCES");
        }
    }

    public class ResourceMap : IEntityTypeConfiguration<Resources>
    {
        public void Configure(EntityTypeBuilder<Resources> entity)
        {
            entity.ToTable("RESOURCES", "core");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Name).HasMaxLength(255);

            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasColumnName("URL");
        }
    }

    public class UserMap : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> entity)
        {

            entity.ToTable("USERS", "core");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Email).HasMaxLength(127);

            entity.Property(e => e.FullName).HasColumnType("nvarchar").HasMaxLength(127);

            entity.Property(e => e.Sub)
                .HasMaxLength(127)
                .IsUnicode(false);

            entity.Property(e => e.LoginID)
                .IsRequired()
                .HasMaxLength(127)
                .IsUnicode(false);

            entity.HasMany(d => d.Privilege)
                .WithMany(p => p.User)
                .UsingEntity<Dictionary<string, object>>(
                    "UserPrivileges",
                    l => l.HasOne<Privileges>().WithMany().HasForeignKey("PrivilegeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_USER_PRIVILEGES_PRIVILEGES"),
                    r => r.HasOne<Users>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_USER_PRIVILEGES_USERS"),
                    j =>
                    {
                        j.HasKey("UserId", "PrivilegeId");

                        j.ToTable("USER_PRIVILEGES", "core");
                    });

            entity.HasMany(d => d.Role)
                .WithMany(p => p.User)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRoles",
                    l => l.HasOne<Roles>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_USER_ROLES_ROLES"),
                    r => r.HasOne<Users>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_USER_ROLES_USERS"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");

                        j.ToTable("USER_ROLES", "core");
                    });
        }
    }
    public class RoleMap : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> entity)
        {

            entity.ToTable("ROLES", "core");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.FullName)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Role)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasMany(d => d.Privilege)
                .WithMany(p => p.Role)
                .UsingEntity<Dictionary<string, object>>(
                    "RolePrivileges",
                    l => l.HasOne<Privileges>().WithMany().HasForeignKey("PrivilegeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ROLE_PRIVILEGES_PRIVILEGES"),
                    r => r.HasOne<Roles>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ROLE_PRIVILEGES_ROLES"),
                    j =>
                    {
                        j.HasKey("RoleId", "PrivilegeId");

                        j.ToTable("ROLE_PRIVILEGES", "core");
                    });
        }
    }
}
