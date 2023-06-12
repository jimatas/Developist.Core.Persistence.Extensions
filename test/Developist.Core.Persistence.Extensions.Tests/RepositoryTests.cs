using Developist.Core.Persistence.Extensions.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;

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
    public void Add_ByDefault_RaisesEntityAddedEvent()
    {
        // Arrange
        using var serviceProvider = ServiceProviderHelper.ConfigureServiceProvider(services => services.AddUnitOfWork().WrapUnitOfWork());
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        var repository = unitOfWork.Repository<Person>();

        var entityToAdd = new Person
        {
            GivenName = "John",
            FamilyName = "Smith",
            Age = 30
        };

        Person? entityAdded = null;
        ((RepositoryWrapper<Person>)repository).EntityAdded += (sender, e) =>
        {
            entityAdded = e.Entity;
        };

        // Act
        repository.Add(entityToAdd);

        // Assert
        Assert.IsNotNull(entityAdded);
        Assert.AreSame(entityToAdd, entityAdded);
    }

    [TestMethod]
    public async Task Remove_ByDefault_RaisesEntityRemovedEvent()
    {
        // Arrange
        using var serviceProvider = ServiceProviderHelper.ConfigureServiceProvider(services => services.AddUnitOfWork().WrapUnitOfWork());
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        var repository = unitOfWork.Repository<Person>();

        var entityToRemove = new Person
        {
            GivenName = "John",
            FamilyName = "Smith",
            Age = 30
        };

        repository.Add(entityToRemove);
        await unitOfWork.CompleteAsync();

        Person? entityRemoved = null;
        ((RepositoryWrapper<Person>)repository).EntityRemoved += (sender, e) =>
        {
            entityRemoved = e.Entity;
        };

        // Act
        repository.Remove(entityToRemove);

        // Assert
        Assert.IsNotNull(entityRemoved);
        Assert.AreSame(entityToRemove, entityRemoved);
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
