using Developist.Core.Persistence.Extensions.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Developist.Core.Persistence.Extensions.Tests;

[TestClass]
public class RepositoryTests
{
    [TestMethod]
    public void Initialize_WithNullRepository_ThrowsArgumentNullException()
    {
        // Arrange
        // Act
        var action = () => new RepositoryWrapper<Person>(repository: null!);

        // Assert
        var exception = Assert.ThrowsException<ArgumentNullException>(action);
        Assert.AreEqual("repository", exception.ParamName);
    }

    [TestMethod]
    public async Task SingleOrDefaultAsync_ByDefault_RaisesEntitiesRetrievedEvent()
    {
        // Arrange
        using var serviceProvider = ServiceProviderHelper.ConfigureServiceProvider(services => services.AddUnitOfWork().WrapUnitOfWork());
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        var repository = unitOfWork.Repository<Person>();

        var entitiesRetrievedEventRaised = false;
        ((RepositoryWrapper<Person>)repository).EntitiesRetrieved += (sender, e) =>
        {
            entitiesRetrievedEventRaised = true;
        };

        // Act
        await repository.SingleOrDefaultAsync(criteria: new PredicateFilterCriteria<Person>(p => true));

        // Assert
        Assert.IsTrue(entitiesRetrievedEventRaised);
    }

    [TestMethod]
    public async Task FirstOrDefaultAsync_ByDefault_RaisesEntitiesRetrievedEvent()
    {
        // Arrange
        using var serviceProvider = ServiceProviderHelper.ConfigureServiceProvider(services => services.AddUnitOfWork().WrapUnitOfWork());
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        var repository = unitOfWork.Repository<Person>();

        var entitiesRetrievedEventRaised = false;
        ((RepositoryWrapper<Person>)repository).EntitiesRetrieved += (sender, e) =>
        {
            entitiesRetrievedEventRaised = true;
        };

        // Act
        await repository.FirstOrDefaultAsync();

        // Assert
        Assert.IsTrue(entitiesRetrievedEventRaised);
    }

    [TestMethod]
    public async Task FirstOrDefaultAsync_WithCriteriaParameter_RaisesEntitiesRetrievedEvent()
    {
        // Arrange
        using var serviceProvider = ServiceProviderHelper.ConfigureServiceProvider(services => services.AddUnitOfWork().WrapUnitOfWork());
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        var repository = unitOfWork.Repository<Person>();

        var entitiesRetrievedEventRaised = false;
        ((RepositoryWrapper<Person>)repository).EntitiesRetrieved += (sender, e) =>
        {
            entitiesRetrievedEventRaised = true;
        };

        // Act
        await repository.FirstOrDefaultAsync(criteria: new PredicateFilterCriteria<Person>(p => true));

        // Assert
        Assert.IsTrue(entitiesRetrievedEventRaised);
    }

    [TestMethod]
    public async Task ListAsync_WithPaginatorParameter_RaisesEntitiesRetrievedEvent()
    {
        // Arrange
        using var serviceProvider = ServiceProviderHelper.ConfigureServiceProvider(services => services.AddUnitOfWork().WrapUnitOfWork());
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        var repository = unitOfWork.Repository<Person>();

        var entitiesRetrievedEventRaised = false;
        ((RepositoryWrapper<Person>)repository).EntitiesRetrieved += (sender, e) =>
        {
            entitiesRetrievedEventRaised = true;
        };

        // Act
        await repository.ListAsync(paginator: new SortingPaginator<Person>());

        // Assert
        Assert.IsTrue(entitiesRetrievedEventRaised);
    }

    [TestMethod]
    public async Task ListAsync_WithCriteriaAndPaginatorParameters_RaisesEntitiesRetrievedEvent()
    {
        // Arrange
        using var serviceProvider = ServiceProviderHelper.ConfigureServiceProvider(services => services.AddUnitOfWork().WrapUnitOfWork());
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        var repository = unitOfWork.Repository<Person>();

        var entitiesRetrievedEventRaised = false;
        ((RepositoryWrapper<Person>)repository).EntitiesRetrieved += (sender, e) =>
        {
            entitiesRetrievedEventRaised = true;
        };

        // Act
        await repository.ListAsync(criteria: new PredicateFilterCriteria<Person>(p => true), paginator: new SortingPaginator<Person>());

        // Assert
        Assert.IsTrue(entitiesRetrievedEventRaised);
    }
}
