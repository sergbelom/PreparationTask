using NetTopologySuite.Geometries;
using PreparationTaskService.DataTransfer.Streets.Models;

namespace PreparationTaskService.Services
{
    public class StreetComputingUtility
    {
        public static Coordinate[] CalculateNewPoints(LineString streetGeometry, Coordinate newPoint)
        {
            Coordinate[] newPoints = new Coordinate[streetGeometry.Count + 1];
            if (streetGeometry.Count == 1)
            {
                newPoints = [newPoint, .. streetGeometry.Coordinates];
            }
            else
            {
                var distanceToFisrt = GetDistance(streetGeometry.StartPoint.X, streetGeometry.StartPoint.Y, newPoint.X, newPoint.Y);
                var distanceToLast = GetDistance(streetGeometry.EndPoint.X, streetGeometry.EndPoint.Y, newPoint.X, newPoint.Y);
                if (distanceToLast < distanceToFisrt)
                {
                    // add to tail
                    streetGeometry.Coordinates.CopyTo(newPoints, 0);
                    newPoints[newPoints.Length - 1] = newPoint;
                }
                else
                {
                    //add to head
                    newPoints = [newPoint, .. streetGeometry.Coordinates];
                }
            }
            return newPoints;
        }

        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }
    }
}
