using NUnit.Framework;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using NetTopologySuite.Geometries;
using PreparationTaskService.DAL.Entities;
using PreparationTaskService.DataTransfer.Streets.Models;
using PreparationTaskService.DataTransfer.Streets;
using PreparationTaskService.Services.Interfaces;
using PreparationTaskService.Services;

namespace StreetServiceUnitTests;

[TestFixture]
public class StreetServiceTests
{
    private StreetService _service;
    private ILogger<StreetService> _logger;
    private IStreetServiceDb _serviceDb;
    private IConfiguration _configuration;
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _logger = Substitute.For<ILogger<StreetService>>();
        _serviceDb = Substitute.For<IStreetServiceDb>();
        _configuration = Substitute.For<IConfiguration>();
        _mapper = Substitute.For<IMapper>();

        _service = new StreetService(_logger, _serviceDb, _configuration, _mapper);
    }

    [Test]
    public async Task FetchAndProcessingStreetCreationAsync_ShouldReturnError_WhenNoPointsProvided()
    {
        // Arrange
        var request = new StreetCreateRequestDto
        {
            Name = "Main Street",
            Points = null,
            Capacity = 50
        };

        _mapper.Map<StreetResponseHolderDto>(Arg.Any<(StreetOperationStates, string)>())
            .Returns(new StreetResponseHolderDto
            {
                StreetResponse = new StreetResponseDto
                {
                    State = StreetOperationStates.Error,
                    Message = "Street does not contain points"
                }
            });

        // Act
        var result = await _service.FetchAndProcessingStreetCreationAsync(request);

        // Assert
        Assert.That(StreetOperationStates.Error == result.StreetResponse.State);
        Assert.That("Street does not contain points" == result.StreetResponse.Message);
    }

    [Test]
    public async Task FetchAndProcessingStreetCreationAsync_ShouldReturnError_WhenStreetAlreadyExists()
    {
        // Arrange
        var request = new StreetCreateRequestDto
        {
            Name = "Main Street",
            Points = new List<StreetPoint> { new StreetPoint { Latitude = 10.0, Longitude = 20.0 } },
            Capacity = 50
        };

        var existingStreet = new StreetEntity { Id = 1, Name = "Main Street" };

        _serviceDb.ReadStreetAsync(request.Name).Returns(existingStreet);

        _mapper.Map<StreetResponseHolderDto>(Arg.Any<(StreetOperationStates, string)>())
            .Returns(new StreetResponseHolderDto
            {
                StreetResponse = new StreetResponseDto
                {
                    State = StreetOperationStates.Error,
                    Message = "Street already exists"
                }
            });

        // Act
        var result = await _service.FetchAndProcessingStreetCreationAsync(request);

        // Assert
        Assert.That(StreetOperationStates.Error == result.StreetResponse.State);
        Assert.That("Street already exists" == result.StreetResponse.Message);
    }

    [Test]
    public async Task FetchAndProcessingStreetCreationAsync_ShouldReturnSuccess_WhenStreetCreated()
    {
        // Arrange
        var request = new StreetCreateRequestDto
        {
            Name = "Main Street",
            Points = new List<StreetPoint> { new StreetPoint { Latitude = 10.0, Longitude = 20.0 } },
            Capacity = 50
        };

        _serviceDb.ReadStreetAsync(request.Name).Returns((StreetEntity)null);

        _serviceDb.CreateStreetAsync(request.Name, Arg.Any<LineString>(), request.Capacity)
            .Returns(true);

        _mapper.Map<StreetResponseHolderDto>(Arg.Any<(StreetOperationStates, string)>())
            .Returns(new StreetResponseHolderDto
            {
                StreetResponse = new StreetResponseDto
                {
                    State = StreetOperationStates.Success,
                    Message = "Street created"
                }
            });

        // Act
        var result = await _service.FetchAndProcessingStreetCreationAsync(request);

        // Assert
        Assert.That(StreetOperationStates.Success == result.StreetResponse.State);
        Assert.That("Street created" == result.StreetResponse.Message);
    }

    [Test]
    public async Task FetchAndProcessingStreetDeletionsAsync_ShouldReturnError_WhenStreetDoesNotExist()
    {
        // Arrange
        var request = new StreetDeleteRequestDto { Name = "Main Street" };

        _serviceDb.ReadStreetAsync(request.Name).Returns((StreetEntity)null);

        _mapper.Map<StreetResponseHolderDto>(Arg.Any<(StreetOperationStates, string)>())
            .Returns(new StreetResponseHolderDto
            {
                StreetResponse = new StreetResponseDto
                {
                    State = StreetOperationStates.Error,
                    Message = "Street does not exist"
                }
            });

        // Act
        var result = await _service.FetchAndProcessingStreetDeletionsAsync(request);

        // Assert
        Assert.That(StreetOperationStates.Error == result.StreetResponse.State);
        Assert.That("Street does not exist" == result.StreetResponse.Message);
    }

    [Test]
    public async Task FetchAndProcessingStreetDeletionsAsync_ShouldReturnSuccess_WhenStreetDeleted()
    {
        // Arrange
        var request = new StreetDeleteRequestDto { Name = "Main Street" };

        var street = new StreetEntity { Id = 1, Name = "Main Street" };

        _serviceDb.ReadStreetAsync(request.Name).Returns(street);

        _serviceDb.DeleteStreetAsync(street).Returns(true);

        _mapper.Map<StreetResponseHolderDto>(Arg.Any<(StreetOperationStates, string)>())
            .Returns(new StreetResponseHolderDto
            {
                StreetResponse = new StreetResponseDto
                {
                    State = StreetOperationStates.Success,
                    Message = "Street removed"
                }
            });

        // Act
        var result = await _service.FetchAndProcessingStreetDeletionsAsync(request);

        // Assert
        Assert.That(StreetOperationStates.Success == result.StreetResponse.State);
        Assert.That("Street removed" == result.StreetResponse.Message);
    }

    [Test]
    public async Task FetchAndProcessingAddingNewPointAsync_ShouldReturnError_WhenStreetDoesNotExist()
    {
        // Arrange
        var request = new StreetAddPointRequestDto
        {
            Name = "Main Street",
            NewPoint = new StreetPoint { Latitude = 15.0, Longitude = 25.0 }
        };

        _serviceDb.ReadStreetAsync(request.Name).Returns((StreetEntity)null);

        _mapper.Map<StreetResponseHolderDto>(Arg.Any<(StreetOperationStates, string)>())
            .Returns(new StreetResponseHolderDto
            {
                StreetResponse = new StreetResponseDto
                {
                    State = StreetOperationStates.Error,
                    Message = "Street does not exist"
                }
            });

        // Act
        var result = await _service.FetchAndProcessingAddingNewPointAsync(request);

        // Assert
        Assert.That(StreetOperationStates.Error == result.StreetResponse.State);
        Assert.That("Street does not exist" == result.StreetResponse.Message);
    }
}