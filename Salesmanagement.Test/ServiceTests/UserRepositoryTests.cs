using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salesmanagement.Test.ServiceTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using global::Salesmanagement.Models;
    using global::Salesmanagement.Repositories;
    using Moq;
    using Xunit;

    namespace Salesmanagement.Tests
    {
        public class UserRepositoryTests
        {
            private readonly List<User> _users; 
            private readonly Mock<IUserRepository> _userRepositoryMock;

            public UserRepositoryTests()
            {
                _users = new List<User>();

                _userRepositoryMock = new Mock<IUserRepository>();

                _userRepositoryMock.Setup(repo => repo.GetUserAsync(It.IsAny<string>()))
                                   .ReturnsAsync((string username) => _users.SingleOrDefault(u => u.Username == username));

                _userRepositoryMock.Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
                                   .Returns(Task.CompletedTask)
                                   .Callback((User user) => _users.Add(user));
            }

            [Fact]
            public async Task GetUserAsync_ReturnsUser_WhenUserExists()
            {
                // Arrange
                var username = "testuser";
                var user = new User { Username = username, Password = "password123" };
                _users.Add(user);

                // Act
                var result = await _userRepositoryMock.Object.GetUserAsync(username);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(username, result.Username);
            }

            [Fact]
            public async Task GetUserAsync_ReturnsNull_WhenUserDoesNotExist()
            {
                // Arrange
                var username = "nonexistentuser";

                // Act
                var result = await _userRepositoryMock.Object.GetUserAsync(username);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task AddUserAsync_AddsUserToRepository()
            {
                // Arrange
                var user = new User { Username = "newuser", Password = "newpassword" };

                // Act
                await _userRepositoryMock.Object.AddUserAsync(user);

                // Assert
                Assert.Contains(user, _users);
            }
        }

    }
}
