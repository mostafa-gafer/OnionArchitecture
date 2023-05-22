using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Identity.Authorization;

namespace Identity.Seed
{
    public static class UserCreator
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            //Seed Super Admin.
            if (await userManager.FindByEmailAsync(EmailConstants.SuperAdminEmail) == null)
            {
                await userManager.CreateAsync(new ApplicationUser
                {
                    Id = IdConstants.SuperAdminId,
                    FirstName = "Mostafa",
                    LastName = "Gafer",
                    UserName = EmailConstants.SuperAdminEmail,
                    Email = EmailConstants.SuperAdminEmail,
                    EmailConfirmed = true
                }, "AaZz12!@");

            }

            //Seed Admin.
            if (await userManager.FindByEmailAsync(EmailConstants.AdminEmail) == null)
            {
                await userManager.CreateAsync(new ApplicationUser
                {
                    Id = IdConstants.AdminId,
                    FirstName = "Hassan",
                    LastName = "Gafer",
                    UserName = EmailConstants.AdminEmail,
                    Email = EmailConstants.AdminEmail,
                    EmailConfirmed = true
                }, "AaZz12!@");
            }

            //Seed Technical.
            if (await userManager.FindByEmailAsync(EmailConstants.TechnicalEmail) == null)
            {
                await userManager.CreateAsync(new ApplicationUser
                {
                    Id = IdConstants.TechnicalId,
                    FirstName = "Hassan",
                    LastName = "Gafer",
                    UserName = EmailConstants.TechnicalEmail,
                    Email = EmailConstants.TechnicalEmail,
                    EmailConfirmed = true
                }, "AaZz12!@");
            }

            //var applicationUser = new ApplicationUser
            //{
            //    FirstName = "Mostafa",
            //    LastName = "Gafer",
            //    UserName = "mostafa.fathy85.mf@gmail.com",
            //    Email = "mostafa.fathy85.mf@gmail.com",
            //    EmailConfirmed = true
            //};

            //var user = await userManager.FindByEmailAsync(applicationUser.Email);
            //if (user == null)
            //{
            //    await userManager.CreateAsync(applicationUser, "AaZz12!@");
            //}
        }
    }
}