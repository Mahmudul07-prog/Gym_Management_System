using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.DataSeed
{
    public class IdentityDBContextSeeding
    {
        public static bool SeedData(RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager)
        {
            try
            {
                var hasUsers = _userManager.Users.Any();
                var hasRoles = _roleManager.Roles.Any();

                if (hasUsers && hasRoles) return false;

                if (!hasRoles)
                {
                    var Roles = new List<IdentityRole>()
                    {
                        new() { Name = "SuperAdmin"},
                        new() { Name = "Admin"}
                    };
                    foreach (var role in Roles)
                    {
                        if (!_roleManager.RoleExistsAsync(role.Name!).Result)
                        {
                            _roleManager.CreateAsync(role).Wait();
                        }
                    }
                }
                if (!hasUsers)
                {
                    var mainAdmin = new ApplicationUser()
                    {
                        FirstName = "Mahmudul",
                        LastName = "Hasan",
                        UserName = "AK47",
                        Email = "mahmudulhasan1@gmail.com",
                        PhoneNumber = "01863846300"
                    };
                    _userManager.CreateAsync(mainAdmin, "P@ssw0rd").Wait();
                    _userManager.AddToRoleAsync(mainAdmin, "SuperAdmin").Wait();

                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Faysal",
                        LastName = "Khan",
                        UserName = "Faysal47",
                        Email = "faysal47@gmail.com",
                        PhoneNumber = "01786980665"
                    };
                    _userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
                    _userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Faild ! {ex}");
                return false;
            }
        }
    }
}
