using LibraryManagement.Models;
using LibraryManagement.Models.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Author> Author { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<BookIssue> BookIssues { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Publisher> Publisher { get; set; }

        public DbSet<Series> Series { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<UserPermission> UserPermissions { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<RoleMenuItem> RoleMenuItems { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BookGenre>()
                .HasKey(bg => new { bg.BookId, bg.GenreId });
            builder.Entity<RolePermission>()
        .HasKey(x => new
        {
            x.RoleId,
            x.PermissionId
        });


            builder.Entity<RolePermission>()
                .HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<RolePermission>()
                .HasOne(x => x.Permission)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.Entity<UserPermission>()
                .HasKey(x => new
                {
                    x.UserId,
                    x.PermissionId
                });


            builder.Entity<UserPermission>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);


            builder.Entity<UserPermission>()
                .HasOne(x => x.Permission)
                .WithMany(x => x.UserPermissions)
                .HasForeignKey(x => x.PermissionId);

            builder.Entity<RoleMenuItem>()
                .HasKey(x => new { x.RoleId, x.MenuItemId });

            builder.Entity<RoleMenuItem>()
                .HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RoleMenuItem>()
                .HasOne(x => x.MenuItem)
                .WithMany(x => x.RoleMenuItems)
                .HasForeignKey(x => x.MenuItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Book>()
                .HasOne(b => b.Series)
                .WithMany(s => s.Books)
                .HasForeignKey(b => b.SeriesId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
