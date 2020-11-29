using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Service;
using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Tests.AdminServiceTests
{
    [TestClass]
    public class EditUser_Should
    {
        [TestMethod]
        public async Task EditUserOn_OnlyFirstNameSubmited()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(EditUserOn_OnlyFirstNameSubmited));
            User user = new User()
            {
                Id = 2,
                FirstName = "Jeff",
                UserName = "Jeff",
                Email = "jeff@ridepal.com"
            };

            UserDTO inputDTO = new UserDTO()
            {
                Id = 2,
                FirstName = "Tom"
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Users.AddAsync(user);
                await arrangeContext.SaveChangesAsync();

                var sut = new AdminService(arrangeContext);
                var editDTO = await sut.EditUser(inputDTO);
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);
                var result = assertContext.Users.FirstOrDefaultAsync(u => u.FirstName == inputDTO.FirstName);

                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task EditUserOn_OnlyLastNameSubmited()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(EditUserOn_OnlyLastNameSubmited));
            User user = new User()
            {
                Id = 2,
                LastName = "Topchiev",
                UserName = "Jeff",
                Email = "jeff@ridepal.com"
            };

            UserDTO inputDTO = new UserDTO()
            {
                Id = 2,
                LastName = "Georgiev"
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Users.AddAsync(user);
                await arrangeContext.SaveChangesAsync();

                var sut = new AdminService(arrangeContext);
                var editDTO = await sut.EditUser(inputDTO);
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);
                var result = assertContext.Users.FirstOrDefaultAsync(u => u.LastName == inputDTO.LastName);

                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task EditUserOn_OnlyEmailSubmited()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(EditUserOn_OnlyEmailSubmited));
            User user = new User()
            {
                Id = 2,
                LastName = "Topchiev",
                UserName = "Jeff",
                Email = "jeff@ridepal.com"
            };

            UserDTO inputDTO = new UserDTO()
            {
                Id = 2,
                Email = "tomsilverman@ridepal.com"
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Users.AddAsync(user);
                await arrangeContext.SaveChangesAsync();

                var sut = new AdminService(arrangeContext);
                var editDTO = await sut.EditUser(inputDTO);
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);
                var result = assertContext.Users.FirstOrDefaultAsync(u => u.Email == inputDTO.Email);

                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task EditUserOn_OnlyUsernameSubmited()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(EditUserOn_OnlyUsernameSubmited));
            User user = new User()
            {
                Id = 2,
                UserName = "regularUser",
                Email = "jeff@ridepal.com"
            };

            UserDTO inputDTO = new UserDTO()
            {
                Id = 2,
                UserName = "OmegaUser"
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Users.AddAsync(user);
                await arrangeContext.SaveChangesAsync();

                var sut = new AdminService(arrangeContext);
                var editDTO = await sut.EditUser(inputDTO);
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);
                var result = assertContext.Users.FirstOrDefaultAsync(u => u.UserName == inputDTO.UserName);

                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task EditUserOn_OnlyPasswordSubmited()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(EditUserOn_OnlyPasswordSubmited));
            var hasher = new PasswordHasher<User>();

            User user = new User()
            {
                Id = 2,
                UserName = "regularUser",
                Email = "jeff@ridepal.com",
            };
            user.PasswordHash = hasher.HashPassword(user, "unbreakablepass");

            UserDTO inputDTO = new UserDTO()
            {
                Id = 2,
                Password = "EvenStrongerPassword"
            };
            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Users.AddAsync(user);
                await arrangeContext.SaveChangesAsync();

                var sut = new AdminService(arrangeContext);
                var editDTO = await sut.EditUser(inputDTO);
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);
                var result = assertContext.Users.FirstOrDefaultAsync(u => u.UserName == inputDTO.UserName);

                var expected = hasher.VerifyHashedPassword(user, user.PasswordHash, inputDTO.Password);

                Assert.IsTrue(expected == PasswordVerificationResult.Success);
            }
        }

        [TestMethod]
        public async Task ThrowException_OnUserNotFound()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ThrowException_OnUserNotFound));

            UserDTO inputDTO = new UserDTO()
            {
                Id = 2,
                UserName = "OmegaUser"
            };

            //Act & Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.EditUser(inputDTO));
            }
        }
    }
}
