﻿using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name = "StaffGo"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Pilot"},
                new AppRole{Name = "FlightAttendant"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                Name = "admin",
                UserName = "admin",
                Email = "admin@vietjetair.com",
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin" });
        }
    }
}
