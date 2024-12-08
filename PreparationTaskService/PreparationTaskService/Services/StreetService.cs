using NetTopologySuite.Geometries;
using PreparationTaskService.DataTransfer.Streets;
using PreparationTaskService.DataTransfer.Streets.Models;

namespace PreparationTaskService.Services
{
    public class StreetService : IStreetService
    {
        private readonly ILogger<StreetService> _logger;
        private readonly IStreetServiceDb _serviceDb;

        public StreetService(ILogger<StreetService> logger, IStreetServiceDb serviceDb)
        {
            _logger = logger;
            _serviceDb = serviceDb;
        }

        public async Task<StreetResponseDto> FetchAndProcessingStreetCreationAsync(StreetCreateRequestDto request)
        {
            var result = await _serviceDb.CreateStreetAsync(request.Name, Map(request.Points), request.Capacity);
            if (result)
            {
                return new StreetResponseDto() { Successfully = true };
            }
            return new StreetResponseDto() { Successfully = false };
        }

        public async Task<StreetResponseDto> FetchAndProcessingStreetDeletionsAsync(StreetDeleteRequestDto request)
        {
            var streets = await _serviceDb.ReadStreetAsync(request.Name);
            var result = await _serviceDb.DeleteStreetAsync(streets);
            if (result)
            {
                return new StreetResponseDto() { Successfully = true };
            }
            return new StreetResponseDto() { Successfully = false };
        }

        public async Task<StreetResponseDto> FetchAndProcessingAddingNewPointAsync(StreetAddPointRequestDto streetAddPointRequest)
        {
            var street = await _serviceDb.ReadStreetAsync(streetAddPointRequest.Name);

            //TOOD: check if point more then 1

            var newX = streetAddPointRequest.NewPoint.X;
            var newY = streetAddPointRequest.NewPoint.Y;

            var distanceToFisrt = GetDistance(street.Geometry.StartPoint.X, street.Geometry.StartPoint.Y, newX, newY);
            var distanceToLast = GetDistance(street.Geometry.EndPoint.X, street.Geometry.EndPoint.Y, newX, newY);

            List<Coordinate> newPoints = null;
            if (distanceToLast < distanceToFisrt)
            {
                newPoints = new List<Coordinate>(street.Geometry.Coordinates) { new Coordinate(newX, newY) };
            }
            else
            {
                newPoints = [new Coordinate(newX, newY), .. street.Geometry.Coordinates];
            }

            var result = await _serviceDb.AddNewPointAsync(street.Id, newPoints.ToArray());
            if (result)
            {
                return new StreetResponseDto() { Successfully = true };
            }
            return new StreetResponseDto() { Successfully = false };
        }

        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        private LineString Map(IEnumerable<StreetPoint> streetPoints)
        {
            //TODO: we need sort streetPoints
            var coordinates = new Coordinate[streetPoints.Count()];
            for (int i = 0; i < streetPoints.Count(); i++)
            {
                coordinates[i] = new Coordinate(streetPoints.ElementAt(i).X, streetPoints.ElementAt(i).Y);
            }
            return new LineString(coordinates);
        }
    }
}
