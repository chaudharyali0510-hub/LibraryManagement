

//using LibraryManagement.Models.Identity;
//using LibraryManagement.Repository.IRepository;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;


//namespace LibraryManagement.Data
//{
//    public static class DbInitializer
//    {
//        public static async Task InitializeAsync(IServiceProvider serviceProvider)
//        {
//            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
//            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

//            if (!context.MenuItems.Any())
//            {
//                var menuItems = new List<MenuItem>
//                {
//                    new() { Name = "Dashboard",      Controller = "Home",      Action = "Index", IconClass = "fas fa-home",           SortOrder = 1 },
//                    new() { Name = "Books",           Controller = "Book",      Action = "Index", IconClass = "fas fa-book",           SortOrder = 2 },
//                    new() { Name = "Authors",         Controller = "Author",    Action = "Index", IconClass = "fas fa-pen-fancy",      SortOrder = 3 },
//                    new() { Name = "Publishers",      Controller = "Publisher", Action = "Index", IconClass = "fas fa-building",       SortOrder = 4 },
//                    new() { Name = "Genres",          Controller = "Genre",     Action = "Index", IconClass = "fas fa-tags",           SortOrder = 5 },
//                    new() { Name = "Series",          Controller = "Series",    Action = "Index", IconClass = "fas fa-layer-group",    SortOrder = 6 },
//                    new() { Name = "Members",         Controller = "Member",    Action = "Index", IconClass = "fas fa-users",          SortOrder = 7 },
//                    new() { Name = "Issue / Return",  Controller = "BookIssue", Action = "Index", IconClass = "fas fa-exchange-alt",   SortOrder = 8 },
//                    new() { Name = "Role Management", Controller = "Role",      Action = "Index", IconClass = "fas fa-user-shield",    SortOrder = 9 },
//                    new() { Name = "User Management", Controller = "User",      Action = "Index", IconClass = "fas fa-user-cog",       SortOrder = 10 },
//                    new() { Name = "Menu Config",     Controller = "Menu",      Action = "Index", IconClass = "fas fa-bars",           SortOrder = 11 },
//                };

//                context.MenuItems.AddRange(menuItems);
//                await context.SaveChangesAsync();
//            }

//            if (!await roleManager.RoleExistsAsync("Admin"))
//            {
//                await roleManager.CreateAsync(new ApplicationRole
//                {
//                    Name = "Admin",
//                    NormalizedName = "ADMIN",
//                    Description = "Full access administrator"
//                });
//            }

//            if (!await roleManager.RoleExistsAsync("Member"))
//            {
//                await roleManager.CreateAsync(new ApplicationRole
//                {
//                    Name = "Member",
//                    NormalizedName = "MEMBER",
//                    Description = "Regular library member"
//                });
//            }

//            var adminEmail = "admin@library.com";
//            if (await userManager.FindByEmailAsync(adminEmail) == null)
//            {
//                var adminUser = new ApplicationUser
//                {
//                    UserName = adminEmail,
//                    Email = adminEmail,
//                    FullName = "System Admin",
//                    EmailConfirmed = true
//                };

//                var result = await userManager.CreateAsync(adminUser, "Admin@123");
//                if (result.Succeeded)
//                {
//                    await userManager.AddToRoleAsync(adminUser, "Admin");
//                }
//            }

//            if (!context.Permissions.Any())
//            {
//                var permissions = new List<Permission>
//                {
//                    new() { Name = "Dashboard.View",   Module = "Dashboard" },
//                    new() { Name = "Books.View",        Module = "Books" },
//                    new() { Name = "Books.Create",      Module = "Books" },
//                    new() { Name = "Books.Edit",        Module = "Books" },
//                    new() { Name = "Books.Delete",      Module = "Books" },
//                    new() { Name = "Authors.View",      Module = "Authors" },
//                    new() { Name = "Authors.Create",    Module = "Authors" },
//                    new() { Name = "Authors.Edit",      Module = "Authors" },
//                    new() { Name = "Authors.Delete",    Module = "Authors" },
//                    new() { Name = "Publishers.View",   Module = "Publishers" },
//                    new() { Name = "Publishers.Create", Module = "Publishers" },
//                    new() { Name = "Publishers.Edit",   Module = "Publishers" },
//                    new() { Name = "Publishers.Delete", Module = "Publishers" },
//                    new() { Name = "Genres.View",       Module = "Genres" },
//                    new() { Name = "Genres.Create",     Module = "Genres" },
//                    new() { Name = "Genres.Edit",       Module = "Genres" },
//                    new() { Name = "Genres.Delete",     Module = "Genres" },
//                    new() { Name = "Members.View",      Module = "Members" },
//                    new() { Name = "Members.Create",    Module = "Members" },
//                    new() { Name = "Members.Edit",      Module = "Members" },
//                    new() { Name = "Members.Delete",    Module = "Members" },
//                    new() { Name = "Issues.View",       Module = "Issues" },
//                    new() { Name = "Issues.Create",     Module = "Issues" },
//                    new() { Name = "Issues.Return",     Module = "Issues" },
//                    new() { Name = "Issues.Delete",     Module = "Issues" },
//                    new() { Name = "Roles.View",        Module = "System" },
//                    new() { Name = "Roles.Create",      Module = "System" },
//                    new() { Name = "Roles.Edit",        Module = "System" },
//                    new() { Name = "Roles.Delete",      Module = "System" },
//                    new() { Name = "Users.View",        Module = "System" },
//                    new() { Name = "Users.Manage",      Module = "System" },
//                    new() { Name = "Menus.View",        Module = "System" },
//                    new() { Name = "Menus.Create",      Module = "System" },
//                    new() { Name = "Menus.Edit",        Module = "System" },
//                    new() { Name = "Menus.Delete",      Module = "System" },
//                    new() { Name = "Menus.Assign",      Module = "System" },
//                    new() { Name = "Series.View",        Module = "Series" },
//                    new() { Name = "Series.Create",      Module = "Series" },
//                    new() { Name = "Series.Edit",        Module = "Series" },
//                    new() { Name = "Series.Delete",      Module = "Series" },
//                };

//                context.Permissions.AddRange(permissions);
//                await context.SaveChangesAsync();
//            }

//            var adminRole = await roleManager.FindByNameAsync("Admin");
//            if (adminRole != null)
//            {
//                var allPermissions = await context.Permissions.ToListAsync();
//                var existingAdminPerms = await context.RolePermissions
//                    .Where(rp => rp.RoleId == adminRole.Id)
//                    .Select(rp => rp.PermissionId)
//                    .ToListAsync();

//                var missingAdminPerms = allPermissions
//                    .Where(p => !existingAdminPerms.Contains(p.Id))
//                    .ToList();

//                if (missingAdminPerms.Any())
//                {
//                    foreach (var perm in missingAdminPerms)
//                    {
//                        context.RolePermissions.Add(new RolePermission
//                        {
//                            RoleId = adminRole.Id,
//                            PermissionId = perm.Id
//                        });
//                    }
//                    await context.SaveChangesAsync();
//                }

//                var allMenuItems = await context.MenuItems.ToListAsync();
//                var existingMenuAssignments = await context.RoleMenuItems
//                    .Where(rm => rm.RoleId == adminRole.Id)
//                    .Select(rm => rm.MenuItemId)
//                    .ToListAsync();

//                var missingMenuAssignments = allMenuItems
//                    .Where(m => !existingMenuAssignments.Contains(m.Id))
//                    .ToList();

//                if (missingMenuAssignments.Any())
//                {
//                    foreach (var menu in missingMenuAssignments)
//                    {
//                        context.RoleMenuItems.Add(new RoleMenuItem
//                        {
//                            RoleId = adminRole.Id,
//                            MenuItemId = menu.Id
//                        });
//                    }
//                    await context.SaveChangesAsync();
//                }
//            }

//            var memberRole = await roleManager.FindByNameAsync("Member");
//            if (memberRole != null)
//            {
//                var viewPermissionNames = new[]
//                {
//                    "Dashboard.View",
//                    "Books.View", "Authors.View", "Publishers.View",
//                    "Genres.View", "Series.View", "Members.View", "Issues.View"
//                };

//                var viewPermissions = await context.Permissions
//                    .Where(p => viewPermissionNames.Contains(p.Name))
//                    .ToListAsync();

//                var existingMemberPerms = await context.RolePermissions
//                    .Where(rp => rp.RoleId == memberRole.Id)
//                    .Select(rp => rp.PermissionId)
//                    .ToListAsync();

//                var missingMemberPerms = viewPermissions
//                    .Where(p => !existingMemberPerms.Contains(p.Id))
//                    .ToList();

//                if (missingMemberPerms.Any())
//                {
//                    foreach (var perm in missingMemberPerms)
//                    {
//                        context.RolePermissions.Add(new RolePermission
//                        {
//                            RoleId = memberRole.Id,
//                            PermissionId = perm.Id
//                        });
//                    }
//                    await context.SaveChangesAsync();
//                }

//                var memberMenuNames = new[] { "Dashboard", "Books", "Authors", "Publishers", "Genres", "Series", "Members", "Issue / Return" };
//                var memberMenuItems = await context.MenuItems
//                    .Where(m => memberMenuNames.Contains(m.Name))
//                    .ToListAsync();

//                var existingMemberMenus = await context.RoleMenuItems
//                    .Where(rm => rm.RoleId == memberRole.Id)
//                    .Select(rm => rm.MenuItemId)
//                    .ToListAsync();

//                var missingMemberMenus = memberMenuItems
//                    .Where(m => !existingMemberMenus.Contains(m.Id))
//                    .ToList();

//                if (missingMemberMenus.Any())
//                {
//                    foreach (var menu in missingMemberMenus)
//                    {
//                        context.RoleMenuItems.Add(new RoleMenuItem
//                        {
//                            RoleId = memberRole.Id,
//                            MenuItemId = menu.Id
//                        });
//                    }
//                    await context.SaveChangesAsync();
//                }
//            }

//            if (!context.MenuItems.Any(m => m.Name == "Series"))
//            {
//                context.MenuItems.Add(new MenuItem
//                {
//                    Name = "Series",
//                    Controller = "Series",
//                    Action = "Index",
//                    IconClass = "fas fa-layer-group",
//                    SortOrder = 6
//                });
//                await context.SaveChangesAsync();

//                var itemsToShift = await context.MenuItems
//                    .Where(m => m.Name != "Series" && m.SortOrder >= 6)
//                    .ToListAsync();
//                foreach (var item in itemsToShift)
//                {
//                    item.SortOrder += 1;
//                }
//                await context.SaveChangesAsync();

//                if (adminRole != null)
//                {
//                    var seriesMenu = await context.MenuItems.FirstOrDefaultAsync(m => m.Name == "Series");
//                    if (seriesMenu != null)
//                    {
//                        var hasSeriesMenu = await context.RoleMenuItems
//                            .AnyAsync(rm => rm.RoleId == adminRole.Id && rm.MenuItemId == seriesMenu.Id);
//                        if (!hasSeriesMenu)
//                        {
//                            context.RoleMenuItems.Add(new RoleMenuItem
//                            {
//                                RoleId = adminRole.Id,
//                                MenuItemId = seriesMenu.Id
//                            });
//                            await context.SaveChangesAsync();
//                        }
//                    }
//                }

//                if (memberRole != null)
//                {
//                    var seriesMenu = await context.MenuItems.FirstOrDefaultAsync(m => m.Name == "Series");
//                    if (seriesMenu != null)
//                    {
//                        var hasSeriesMenu = await context.RoleMenuItems
//                            .AnyAsync(rm => rm.RoleId == memberRole.Id && rm.MenuItemId == seriesMenu.Id);
//                        if (!hasSeriesMenu)
//                        {
//                            context.RoleMenuItems.Add(new RoleMenuItem
//                            {
//                                RoleId = memberRole.Id,
//                                MenuItemId = seriesMenu.Id
//                            });
//                            await context.SaveChangesAsync();
//                        }
//                    }
//                }
//            }

//            var seriesPermNames = new[] { "Series.View", "Series.Create", "Series.Edit", "Series.Delete" };
//            var existingSeriesPerms = await context.Permissions
//                .Where(p => seriesPermNames.Contains(p.Name))
//                .Select(p => p.Id)
//                .ToListAsync();

//            if (existingSeriesPerms.Count < 4)
//            {
//                var allPerms = await context.Permissions.ToListAsync();
//                var missingSeriesPerms = seriesPermNames
//                    .Where(name => !allPerms.Any(p => p.Name == name))
//                    .Select(name => new Permission { Name = name, Module = "Series" })
//                    .ToList();

//                if (missingSeriesPerms.Any())
//                {
//                    context.Permissions.AddRange(missingSeriesPerms);
//                    await context.SaveChangesAsync();
//                }

//                if (adminRole != null)
//                {
//                    var freshSeriesPerms = await context.Permissions
//                        .Where(p => seriesPermNames.Contains(p.Name))
//                        .ToListAsync();
//                    var existingAdminSeriesPerms = await context.RolePermissions
//                        .Where(rp => rp.RoleId == adminRole.Id && freshSeriesPerms.Select(p => p.Id).Contains(rp.PermissionId))
//                        .Select(rp => rp.PermissionId)
//                        .ToListAsync();

//                    foreach (var perm in freshSeriesPerms.Where(p => !existingAdminSeriesPerms.Contains(p.Id)))
//                    {
//                        context.RolePermissions.Add(new RolePermission
//                        {
//                            RoleId = adminRole.Id,
//                            PermissionId = perm.Id
//                        });
//                    }
//                    await context.SaveChangesAsync();
//                }

//                if (memberRole != null)
//                {
//                    var seriesViewPerm = await context.Permissions.FirstOrDefaultAsync(p => p.Name == "Series.View");
//                    if (seriesViewPerm != null)
//                    {
//                        var hasPerm = await context.RolePermissions
//                            .AnyAsync(rp => rp.RoleId == memberRole.Id && rp.PermissionId == seriesViewPerm.Id);
//                        if (!hasPerm)
//                        {
//                            context.RolePermissions.Add(new RolePermission
//                            {
//                                RoleId = memberRole.Id,
//                                PermissionId = seriesViewPerm.Id
//                            });
//                            await context.SaveChangesAsync();
//                        }
//                    }
//                }
//            }
//        }
//    }
//}
