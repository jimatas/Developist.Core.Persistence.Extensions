using Developist.Core.Persistence.Extensions.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reflection;

namespace Developist.Core.Persistence.Extensions.Tests;

[TestClass]
public class UnitOfWorkTests
{
    [TestMethod]
    public void Initialize_WithNullUnitOfWork_ThrowsArgumentNullException()
    {
        // Arrange
        // Act
        var action = () => new UnitOfWorkWrapper(unitOfWork: null!);

        // Assert
        var exception = Assert.ThrowsException<ArgumentNullException>(action);
        Assert.AreEqual("unitOfWork", exception.ParamName);
    }

    [TestMethod]
    public void Repository_ByDefault_ReturnsRepositoryWrapper()
    {
        // Arrange
        using var serviceProvider = ServiceProviderHelper.ConfigureServiceProvider(services => services.AddUnitOfWork().WrapUnitOfWork());
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        // Act
        var repository = unitOfWork.Repository<Person>();

        // Assert
        Assert.IsInstanceOfType(repository, typeof(RepositoryWrapper<Person>));
    }

    [TestMethod]
    public void Repository_CalledOnUnitOfWorkWrapper_ReturnsRepositoryWrapper()
    {
        // Arrange
        using var serviceProvider = ServiceProviderHelper.ConfigureServiceProvider(services => services.AddUnitOfWork().WrapUnitOfWork());
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        // Act
        var repository = ((UnitOfWorkWrapper)unitOfWork).Repository<Person>();

        // Assert
        Assert.IsInstanceOfType(repository, typeof(RepositoryWrapper<Person>));
    }

    [TestMethod]
    public void HasActiveTransaction_ByDefault_DelegatesToWrappedUnitOfWork()
    {
        // Arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(uow => uow.HasActiveTransaction).Verifiable();

        var unitOfWork = new UnitOfWorkWrapper(mockUnitOfWork.Object);

        // Act
        _ = unitOfWork.HasActiveTransaction;

        // Assert
        mockUnitOfWork.Verify();
    }

    [TestMethod]
    public async Task CompletedEvent_ByDefault_DelegatesToWrappedUnitOfWork()
    {
        // Arrange
        using var serviceProvider = ServiceProviderHelper.ConfigureServiceProvider(services => services.AddUnitOfWork().WrapUnitOfWork());
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        var wrappedUnitOfWork = (IUnitOfWork)unitOfWork.GetType()
            .GetProperty("UnitOfWork", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(unitOfWork)!;

        var completeEventDelegated = false;

        unitOfWork.Completed += (sender, e) =>
        {
            completeEventDelegated = sender == wrappedUnitOfWork;
        };

        // Act
        await unitOfWork.CompleteAsync();

        // Assert
        Assert.IsTrue(completeEventDelegated);
    }

    [TestMethod]
    public async Task BeginTransactionAsync_ByDefault_DelegatesToWrappedUnitOfWork()
    {
        // Arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>())).Verifiable();

        var unitOfWork = new UnitOfWorkWrapper(mockUnitOfWork.Object);

        // Act
        await unitOfWork.BeginTransactionAsync();

        // Assert
        mockUnitOfWork.Verify();
    }
}
