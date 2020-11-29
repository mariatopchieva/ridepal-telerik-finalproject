using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Tests.AdminServiceTests
{
    [TestClass]
    public class UnbanUser_Should
    {
        [TestMethod]
        public async Task SetLockoutStatus_ToFalseCurrectly()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(SetLockoutStatus_ToFalseCurrectly));

            User user = new User
            {
                Id = 1,
                UserName = "tomas",
                Email = "tomas@ridepal.com",
                LockoutEnabled = true
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Users.AddAsync(user);
                await arrangeContext.SaveChangesAsync();

                var sut = new AdminService(arrangeContext);
                bool unbanResult = await sut.UnbanUserById(1);
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var userFromDb = await assertContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

                Assert.IsFalse(userFromDb.LockoutEnabled);
            }
        }

        [TestMethod]
        public async Task ReturnFalse_OnUserNotFoundInDB()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnFalse_OnUserNotFoundInDB));

            //Act & Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);
                bool result = await sut.UnbanUserById(1);

                Assert.IsFalse(result);
            }
        }
    }
}
