using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ECommerce.Persistence.IdentityData.DataSeed
{
    public class IdentityDataInitializer : IDataInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataInitializer> _logger;

        public IdentityDataInitializer(UserManager<ApplicationUser> userManager,
                                       RoleManager<IdentityRole> roleManager,
                                       ILogger<IdentityDataInitializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SumperAdmin"));
                }

                if (!_userManager.Users.Any())
                {
                    var user1 = new ApplicationUser
                    {
                        DisplayName = "Yehia NourEldin",
                        UserName = "YehiaNourEldin",
                        Email = "YehiaNourEldin@gmail.com",
                        PhoneNumber = "01142394329"
                    };
                    var user2 = new ApplicationUser
                    {
                        DisplayName = "Ali Ibrahim",
                        UserName = "AliIbrahim",
                        Email = "AliIbrahim@gmail.com",
                        PhoneNumber = "01142394322"
                    };

                    await _userManager.CreateAsync(user1, "P@ssw0rd");
                    await _userManager.CreateAsync(user2, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(user1, "SumperAdmin");
                    await _userManager.AddToRoleAsync(user2, "Admin");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while seeding identity database : Message = {ex.Message} ");
            }
        }
    }
}
