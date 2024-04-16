using Microsoft.AspNetCore.Identity;
using RentalsWebApp.Data.Enums;
using RentalsWebApp.Models;

namespace RentalsWebApp.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDBContext>();

                if (!context.Apartments.Any())
                {
                    context.Apartments.AddRange(new List<Apartments>()
                    {
                        new Apartments()
                        {
                           Description = "3 Bedroom house with one bath and a car port",
                           ApartmentCategory = ApartmentCategory.Three_Bedroom,
                           ApartmentPictures = new ApartmentPictures()
                           {
                               Pic1 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic2 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic3 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic4 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic5 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic6 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg"

                           },

                           Address = new Address()
                           {
                               Address_Line1 = "92 John Drive",
                               Address_Line2 = "6 Ridgeway Gardens",
                               City = "Mondeor, Johannesburg",
                               State = States.Gauteng,
                               Zip = "1123"
                           },
                           Price = "7500"

                        },
                        new Apartments()
                        {
                           Description = "3 Bedroom house with one bath and a garage",
                           ApartmentCategory = ApartmentCategory.Three_Bedroom,
                           ApartmentPictures = new ApartmentPictures()
                           {
                               Pic1 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic2 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic3 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic4 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic5 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic6 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg"

                           },
                           Address = new Address()
                           {
                               Address_Line1 = "70 Bela Drive",
                               Address_Line2 = "3 Belarose",
                               City = "Bela, Johannesburg",
                               State = States.Gauteng,
                               Zip = "1123"
                           },
                           Price = "8000"
                        },
                        new Apartments()
                        {
                           Description = "2 Bedroom house with one bath and a car port",
                           ApartmentCategory = ApartmentCategory.Two_Bedroom,
                           ApartmentPictures = new ApartmentPictures()
                           {
                               Pic1 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic2 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic3 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic4 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic5 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic6 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg"

                           },
                           Address = new Address()
                           {
                               Address_Line1 = "88 Wind Drive",
                               Address_Line2 = "2 Windmill",
                               City = "Bocksburg, Johannesburg",
                               State = States.Gauteng,
                               Zip = "1123"
                           },
                           Price = "5500"

                        },
                        new Apartments()
                        {
                           Description = "A single room with one bath",
                           ApartmentCategory = ApartmentCategory.Single,
                          ApartmentPictures = new ApartmentPictures()
                           {
                               Pic1 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic2 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic3 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic4 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic5 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg",
                               Pic6 = "https://www.forbes.com/advisor/wp-content/uploads/2022/06/Image_-_Copyright_.jpeg.jpg"

                           },
                           Address = new Address()
                           {
                               Address_Line1 = "5 Regen Drive",
                               Address_Line2 = "83 Regentspack",
                               City = "Regentspack, Johannesburg",
                               State = States.Gauteng,
                               Zip = "1123"
                           },
                           Price = "3500"

                        }

                    });
                    context.SaveChanges();
                }

            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Tenant))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Tenant));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "nokiechauke@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "nokieadmin",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Name = "Nokie",
                        Surname = "Chauke",
                        PhoneNumber = "0814259632",
                        PhoneNumberConfirmed = true,
                        IdentityNo = "9000001111102"

                    };
                    await userManager.CreateAsync(newAdminUser, "P@ssw0rd1");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
            }
        }

    }
}

