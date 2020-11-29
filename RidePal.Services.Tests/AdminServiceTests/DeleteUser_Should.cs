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
    public class DeleteUser_Should
    {
        [TestMethod]
        public async Task SetDeletedStatus_Currectly()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(SetDeletedStatus_Currectly));

            User user = new User
            {
                Id = 1,
                UserName = "tom",
                Email = "tom@ridepal.com"
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Users.AddAsync(user);
                await arrangeContext.SaveChangesAsync();

                var sut = new AdminService(arrangeContext);
                bool delResult = await sut.DeleteUser(1);
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var userFromDb = await assertContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

                Assert.IsTrue(userFromDb.IsDeleted);
            }
        }

        [TestMethod]
        public async Task ReturnFalse_OnUserToDeleteNotFound()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnFalse_OnUserToDeleteNotFound));

            //Act & Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);
                bool result = await sut.DeleteUser(1);

                Assert.IsFalse(result);
            }
        }
    }
}
