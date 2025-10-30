using System.ComponentModel.DataAnnotations;
using DogsHouse.Application.DTOs;
using DogsHouse.Application.Filters;
using DogsHouse.Application.Interfaces.Repositories;
using DogsHouse.Application.Models;
using DogsHouse.Application.Services;
using DogsHouse.Infrastructure.MSSQL.Data;
using DogsHouse.Infrastructure.MSSQL.DbContext;
using Microsoft.EntityFrameworkCore;
using Moq;
using Testcontainers.MsSql;

namespace DogsHouse.Application.Tests.Services;

public class DogsServiceTests : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Strong@Passw0rd")
        .Build();

    private DogsHouseDbContext _context = null!;
    private DogsService _dogsService = null!;

    private UnitOfWork _unitOfWork => new UnitOfWork(_context);

    public async Task InitializeAsync()
    {
        // Start the container
        await _msSqlContainer.StartAsync();

        // Configure DbContext using container connection string
        var options = new DbContextOptionsBuilder<DogsHouseDbContext>()
            .UseSqlServer(_msSqlContainer.GetConnectionString())
            .Options;

        _context = new DogsHouseDbContext(options);

        // Ensure the database schema is created
        await _context.Database.EnsureCreatedAsync();

        // Initialize service
        _dogsService = new DogsService(_unitOfWork);
    }

    public Task DisposeAsync()
    {
        return _msSqlContainer.StopAsync();
    }

    [Fact]
    public async Task GetAllDogsAsync_ReturnsEmpty_WhenNoDogs()
    {
        // Arrange
        var mockRepo = new Mock<IDogsRepository>();
        mockRepo
            .Setup(r => r.GetAllDogsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<Dog>());

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork
            .SetupGet(u => u.DogsRepository)
            .Returns(mockRepo.Object);

        var service = new DogsService(mockUnitOfWork.Object);

        // Act
        var result = await service.GetAllDogsAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        mockRepo.Verify(r => r.GetAllDogsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllDogsAsync_ReturnsMappedDogDtos_WhenDogsExist()
    {
        // Arrange
        var dogs = new List<Dog>
            {
                new Dog { Name = "Rex", Color = "Brown", TailLength = 10, Weight = 20 },
                new Dog { Name = "Bella", Color = "Black", TailLength = 8, Weight = 15 }
            };

        var mockRepo = new Mock<IDogsRepository>();
        mockRepo
            .Setup(r => r.GetAllDogsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dogs);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork
            .SetupGet(u => u.DogsRepository)
            .Returns(mockRepo.Object);

        var service = new DogsService(mockUnitOfWork.Object);

        // Act
        var result = (await service.GetAllDogsAsync(CancellationToken.None)).ToList();

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Equal(dogs[0].Name, result[0].Name);
        Assert.Equal(dogs[0].Color, result[0].Color);
        Assert.Equal(dogs[0].TailLength, result[0].TailLength);
        Assert.Equal(dogs[0].Weight, result[0].Weight);

        Assert.Equal(dogs[1].Name, result[1].Name);
        Assert.Equal(dogs[1].Color, result[1].Color);
        Assert.Equal(dogs[1].TailLength, result[1].TailLength);
        Assert.Equal(dogs[1].Weight, result[1].Weight);

        mockRepo.Verify(r => r.GetAllDogsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Adding_Two_Dogs_With_Same_Name_Should_Fail_Validation()
    {
        // Arrange
        var dogName = "Rex";

        var request1 = new AddDogRequest { Name = dogName, Color = "Brown", TailLength = 5, Weight = 10 };
        var request2 = new AddDogRequest { Name = dogName, Color = "Black", TailLength = 6, Weight = 12 };

        var mockRepo = new Mock<IDogsRepository>();
        // First validation -> name does not exist, Second -> name exists
        mockRepo
            .SetupSequence(r => r.ExistsByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false)
            .ReturnsAsync(true);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork
            .SetupGet(u => u.DogsRepository)
            .Returns(mockRepo.Object);

        // simple service provider that returns our mock unit of work
        var serviceProvider = new TestServiceProvider(mockUnitOfWork.Object);

        // Act - validate first request
        var results1 = new List<ValidationResult>();
        var context1 = new ValidationContext(request1, serviceProvider, items: null);
        var isValidFirst = Validator.TryValidateObject(request1, context1, results1, validateAllProperties: true);

        // Act - validate second request (should fail because ExistsByNameAsync returns true)
        var results2 = new List<ValidationResult>();
        var context2 = new ValidationContext(request2, serviceProvider, items: null);
        var isValidSecond = Validator.TryValidateObject(request2, context2, results2, validateAllProperties: true);

        // Assert
        Assert.True(isValidFirst);
        Assert.Empty(results1);

        Assert.False(isValidSecond);
        Assert.Single(results2);
        Assert.Contains($"Dog with name '{dogName}' already exists.", results2[0].ErrorMessage);
    }

    [Fact]
    public async Task AddDog_Should_Add_Dog_To_Database()
    {
        // Arrange
        var dog = new AddDogRequest { Name = "Buddy", Color = "Golden", TailLength = 12, Weight = 30 };

        // Act
        await _dogsService.AddDogAsync(dog, CancellationToken.None);

        // Assert
        var dogInDb = await _context.Dogs.FirstOrDefaultAsync(d => d.Name == "Buddy");
        Assert.NotNull(dogInDb);
        Assert.Equal("Buddy", dogInDb.Name);
    }

    [Fact]
    public void Adding_Dog_With_Negative_TailLength_Should_Throw_ValidationException()
    {
        // Arrange
        var request = new AddDogRequest { Name = "Rex", Color = "Brown", TailLength = -1, Weight = 10 };

        var mockRepo = new Mock<IDogsRepository>();
        // ensure UniqueDogName validation does not block the test (name does not exist)
        mockRepo
            .Setup(r => r.ExistsByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork
            .SetupGet(u => u.DogsRepository)
            .Returns(mockRepo.Object);

        var serviceProvider = new TestServiceProvider(mockUnitOfWork.Object);
        var context = new ValidationContext(request, serviceProvider, items: null);

        // Act & Assert - TailLengthNegative validation throws ValidationException
        var ex = Assert.Throws<ValidationException>(() =>
            Validator.ValidateObject(request, context, validateAllProperties: true));

        Assert.Contains("Tail length can be only positive", ex.Message);
    }

    [Fact]
    public async Task GetSortedDogsAsync_ShouldReturn_CorrectlySortedAndPaginatedDogs()
    {
        // Arrange
        _context.Dogs.AddRange(
            new Dog { Name = "Rex", Color = "Brown", TailLength = 10, Weight = 20 },
            new Dog { Name = "Bella", Color = "Black", TailLength = 12, Weight = 25 },
            new Dog { Name = "Charlie", Color = "White", TailLength = 8, Weight = 15 }
        );
        await _context.SaveChangesAsync();

        var filterParams = new FilterParams
        {
            PageNumber = 1,
            PageSize = 2,
            Attribute = "taillength",
            Order = "asc"
        };

        // Act
        var result = await _dogsService.GetSortedDogsAsync(filterParams, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Dogs);
        Assert.NotNull(result);
        Assert.Equal(2, result.Dogs.Count());
        Assert.Equal("Charlie", result.Dogs.First().Name); // lowest age
    }

    // small helper to provide IUnitOfWork to ValidationContext.GetService
    private class TestServiceProvider : IServiceProvider
    {
        private readonly IUnitOfWork _unitOfWork;
        public TestServiceProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public object? GetService(Type serviceType) => serviceType == typeof(IUnitOfWork) ? _unitOfWork : null;
    }
}
