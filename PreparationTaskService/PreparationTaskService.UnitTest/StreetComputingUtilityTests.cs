using NUnit.Framework;
using NetTopologySuite.Geometries;
using PreparationTaskService.Services;

namespace StreetComputingUtilityTests;

[TestFixture]
public class StreetComputingUtilityTests
{
    [Test]
    public void CalculateNewPoints_ShouldAddNewPointToTail_WhenCloserToLastPoint()
    {
        // Arrange
        var streetGeometry = new LineString(new[]
        {
            new Coordinate(0, 0),
            new Coordinate(10, 0)
        });
        var newPoint = new Coordinate(15, 0); // Closer to the last point

        // Act
        var result = StreetComputingUtility.CalculateNewPoints(streetGeometry, newPoint);

        // Assert
        Assert.That(3 == result.Length);
        Assert.That(result[0].X == 0 && result[0].Y == 0);
        Assert.That(result[1].X == 10 && result[1].Y == 0);
        Assert.That(newPoint == result[2]); // New point added to tail
    }

    [Test]
    public void CalculateNewPoints_ShouldAddNewPointToHead_WhenCloserToFirstPoint()
    {
        // Arrange
        var streetGeometry = new LineString(new[]
        {
            new Coordinate(0, 0),
            new Coordinate(10, 0)
        });
        var newPoint = new Coordinate(-5, 0); // Closer to the first point

        // Act
        var result = StreetComputingUtility.CalculateNewPoints(streetGeometry, newPoint);

        // Assert
        Assert.That(3 == result.Length);
        Assert.That(newPoint == result[0]); // New point added to head
        Assert.That(result[1].X == 0 && result[1].Y == 0);
        Assert.That(result[2].X == 10 && result[2].Y == 0);
    }

    [Test]
    public void GetDistance_ShouldReturnCorrectDistance()
    {
        // Arrange
        double x1 = 0, y1 = 0, x2 = 3, y2 = 4;

        // Act
        var result = StreetComputingUtility.GetDistance(x1, y1, x2, y2);

        // Assert
        Assert.That(5 == result); // 3-4-5 triangle
    }
}