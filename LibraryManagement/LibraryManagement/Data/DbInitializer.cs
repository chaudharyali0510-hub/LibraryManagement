using LibraryManagement.Models.Identity;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (!context.MenuItems.Any())
            {
                var menuItems = new List<MenuItem>
                {
                    new() { Name = "Dashboard",      Controller = "Home",      Action = "Index", IconClass = "fas fa-home",           SortOrder = 1 },
                    new() { Name = "Books",           Controller = "Book",      Action = "Index", IconClass = "fas fa-book",           SortOrder = 2 },
                    new() { Name = "Authors",         Controller = "Author",    Action = "Index", IconClass = "fas fa-pen-fancy",      SortOrder = 3 },
                    new() { Name = "Publishers",      Controller = "Publisher", Action = "Index", IconClass = "fas fa-building",       SortOrder = 4 },
                    new() { Name = "Genres",          Controller = "Genre",     Action = "Index", IconClass = "fas fa-tags",           SortOrder = 5 },
                    new() { Name = "Members",         Controller = "Member",    Action = "Index", IconClass = "fas fa-users",          SortOrder = 6 },
                    new() { Name = "Issue / Return",  Controller = "BookIssue", Action = "Index", IconClass = "fas fa-exchange-alt",    SortOrder = 7 },
                    new() { Name = "Role Management", Controller = "Role",      Action = "Index", IconClass = "fas fa-user-shield",    SortOrder = 8 },
                    new() { Name = "User Management", Controller = "User",      Action = "Index", IconClass = "fas fa-user-cog",       SortOrder = 9 },
                    new() { Name = "Menu Config",     Controller = "Menu",      Action = "Index", IconClass = "fas fa-bars",           SortOrder = 10 },
                };

                context.MenuItems.AddRange(menuItems);
                await context.SaveChangesAsync();
            }

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Full access administrator"
                });
            }

            var adminEmail = "admin@library.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");

                    var adminRole = await roleManager.FindByNameAsync("Admin");
                    var allMenuItems = context.MenuItems.ToList();

                    foreach (var menu in allMenuItems)
                    {
                        context.RoleMenuItems.Add(new RoleMenuItem
                        {
                            RoleId = adminRole!.Id,
                            MenuItemId = menu.Id
                        });
                    }

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
