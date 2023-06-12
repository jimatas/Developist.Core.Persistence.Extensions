using Developist.Core.Persistence.Extensions.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;

namespace Developist.Core.Persistence.Extensions.Tests;

[TestClass]
public class DependencyInjectionTests
{
    [TestMethod]
    public void WrapUnitOfWork_ByDefault_UsesUnitOfWorkWrapper()
    {
        // Arrange
        // Act
        using var serviceProvider = ServiceProviderHelper.ConfigureServiceProvider(services => services.AddUnitOfWork().WrapUnitOfWork());
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        // Assert
        Assert.IsInstanceOfType(unitOfWork, typeof(UnitOfWorkWrapper));
    }

    [TestMethod]
    public void WrapUnitOfWork_GivenNullFactory_ThrowsArgumentNullException()
    {
        // Arrange
        // Act
        var action = () => ServiceProviderHelper.ConfigureServiceProvider(services => services.AddUnitOfWork().WrapUnitOfWork(null!));

        // Assert
        var exception = Assert.ThrowsException<ArgumentNullException>(action);
        Assert.AreEqual("factory", exception.ParamName);
    }
}
