using NetTopologySuite.Geometries;
using PreparationTaskService.DataTransfer.Streets;
using PreparationTaskService.DataTransfer.Streets.Models;
using Messages = PreparationTaskService.Common.StaticMessages;

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
            var street = await _serviceDb.ReadStreetAsync(request.Name);
            if (street != null)
            {
                return new StreetResponseDto() { State = StreetOperationStates.Error, Message = Messages.STREET_ALREADY_EXISTING };
            }
            if (null == request.Points || request.Points.Count == 0)
            {
                return new StreetResponseDto() { State = StreetOperationStates.Error, Message = Messages.STREET_DOES_NOT_CONTAIN_POINTS };
            }
            var result = await _serviceDb.CreateStreetAsync(request.Name, Map(request.Points), request.Capacity);
            if (result)
            {
                return new StreetResponseDto() { State = StreetOperationStates.Success, Message = Messages.STREET_CREATED };
            }
            return new StreetResponseDto() { State = StreetOperationStates.Error };
        }

        public async Task<StreetResponseDto> FetchAndProcessingStreetDeletionsAsync(StreetDeleteRequestDto request)
        {
            var street = await _serviceDb.ReadStreetAsync(request.Name);
            if (null == street)
            {
                return new StreetResponseDto() { State = StreetOperationStates.Error, Message = Messages.STREET_NOT_EXISTING };
            }
            var result = await _serviceDb.DeleteStreetAsync(street);
            if (result)
            {
                return new StreetResponseDto() { State = StreetOperationStates.Success, Message = Messages.STREET_REMOVED };
            }
            return new StreetResponseDto() { State = StreetOperationStates.Error };
        }

        public async Task<StreetResponseDto> FetchAndProcessingAddingNewPointAsync(StreetAddPointRequestDto streetAddPointRequest)
        {
            var street = await _serviceDb.ReadStreetAsync(streetAddPointRequest.Name);
            if (street != null)
            {
                var newPoint = new Coordinate(streetAddPointRequest.NewPoint.X, streetAddPointRequest.NewPoint.Y);
                if (!street.Geometry.IsCoordinate(newPoint))
                {
                    var newPoints = StreetComputingUtility.CalculateNewPoints(street.Geometry, newPoint);
                    var result = await _serviceDb.AddNewPointAsync(street.Id, newPoints.ToArray());
                    if (result)
                    {
                        return new StreetResponseDto() { State = StreetOperationStates.Success, Message = Messages.NEW_POINT_ADDED };
                    }
                    else
                    {
                        return new StreetResponseDto() { State = StreetOperationStates.Error };
                    }
                }
                else
                {
                    return new StreetResponseDto() { State = StreetOperationStates.Error, Message = Messages.POINT_ALREADY_EXISTING };
                }
            }
            else
            {
                return new StreetResponseDto() { State = StreetOperationStates.Error, Message = Messages.STREET_NOT_EXISTING };
            }
        }

        private LineString Map(IEnumerable<StreetPoint> streetPoints)
        {
            var coordinates = new Coordinate[streetPoints.Count()];
            for (int i = 0; i < streetPoints.Count(); i++)
            {
                coordinates[i] = new Coordinate(streetPoints.ElementAt(i).X, streetPoints.ElementAt(i).Y);
            }
            return new LineString(coordinates);
        }
    }
}
