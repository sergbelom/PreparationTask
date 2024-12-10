using NUnit.Framework;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using PreparationTaskService.Controller;
using PreparationTaskService.DataTransfer.Streets.Models;
using PreparationTaskService.DataTransfer.Streets;
using PreparationTaskService.Services.Interfaces;
using NSubstitute.ExceptionExtensions;

namespace PreparationTaskControllerUnitTests;

[TestFixture]
public class PreparationTaskControllerTests : IDisposable
{
    private PreparationTaskController _controller;
    private IStreetService _streetService;
    private ILogger<PreparationTaskController> _logger;

    [SetUp]
    public void SetUp()
    {
        _streetService = Substitute.For<IStreetService>();
        _logger = Substitute.For<ILogger<PreparationTaskController>>();
        _controller = new PreparationTaskController(_logger, _streetService);
    }

    [TearDown]
    public void TearDown()
    {
        // Dispose of any IDisposable resources
        _controller?.Dispose();
        (_streetService as IDisposable)?.Dispose();
        (_logger as IDisposable)?.Dispose();
    }

    public void Dispose()
    {
        // Additional cleanup, if needed
        _controller?.Dispose();
        (_streetService as IDisposable)?.Dispose();
        (_logger as IDisposable)?.Dispose();
    }

    [Test]
    public async Task PostStreetCreate_ShouldReturnOkResult_WhenServiceReturnsResponse()
    {
        // Arrange
        var request = new StreetCreateRequestHolderDto
        {
            StreetCreateRequest = new StreetCreateRequestDto
            {
                Name = "Main Street",
                Points = new List<StreetPoint> { new StreetPoint { Latitude = 10.0, Longitude = 20.0 } },
                Capacity = 50
            }
        };

        var response = new StreetResponseHolderDto
        {
            StreetResponse = new StreetResponseDto
            {
                State = StreetOperationStates.Success,
                Message = "Street created successfully"
            }
        };

        _streetService.FetchAndProcessingStreetCreationAsync(request.StreetCreateRequest).Returns(response);

        // Act
        var result = await _controller.PostStreetCreate(request) as OkObjectResult;

        // Assert
        Assert.That(result != null);
        Assert.That(200 == result.StatusCode);
        Assert.That(response == result.Value);
    }

    [Test]
    public async Task PostStreetDelete_ShouldReturnOkResult_WhenServiceReturnsResponse()
    {
        // Arrange
        var request = new StreetDeleteRequestHolderDto
        {
            StreetDeleteRequest = new StreetDeleteRequestDto
            {
                Name = "Main Street"
            }
        };

        var response = new StreetResponseHolderDto
        {
            StreetResponse = new StreetResponseDto
            {
                State = StreetOperationStates.Success,
                Message = "Street deleted successfully"
            }
        };

        _streetService.FetchAndProcessingStreetDeletionsAsync(request.StreetDeleteRequest).Returns(response);

        // Act
        var result = await _controller.PostStreetDelete(request) as OkObjectResult;

        // Assert
        Assert.That(result != null);
        Assert.That(200 == result.StatusCode);
        Assert.That(response == result.Value);
    }

    [Test]
    public async Task PostStreetAddPoint_ShouldReturnOkResult_WhenServiceReturnsResponse()
    {
        // Arrange
        var request = new StreetAddPointRequestHolderDto
        {
            StreetAddPointRequest = new StreetAddPointRequestDto
            {
                Name = "Main Street",
                NewPoint = new StreetPoint { Latitude = 15.0, Longitude = 25.0 }
            }
        };

        var response = new StreetResponseHolderDto
        {
            StreetResponse = new StreetResponseDto
            {
                State = StreetOperationStates.Success,
                Message = "Point added successfully"
            }
        };

        _streetService.FetchAndProcessingAddingNewPointAsync(request.StreetAddPointRequest).Returns(response);

        // Act
        var result = await _controller.PostStreetAddPoint(request) as OkObjectResult;

        // Assert
        Assert.That(result != null);
        Assert.That(200 == result.StatusCode);
        Assert.That(response == result.Value);
    }
}