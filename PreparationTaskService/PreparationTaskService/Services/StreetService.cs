using NetTopologySuite.Geometries;
using PreparationTaskService.DAL.Entities;
using PreparationTaskService.DataTransfer.Streets;
using PreparationTaskService.DataTransfer.Streets.Models;
using static PreparationTaskService.Common.StaticMessages;
using static PreparationTaskService.Common.Statics;

namespace PreparationTaskService.Services
{
    public class StreetService : IStreetService
    {
        private readonly ILogger<StreetService> _logger;
        private readonly IStreetServiceDb _serviceDb;
        private readonly IConfiguration _configuration;

        public StreetService(ILogger<StreetService> logger, IStreetServiceDb serviceDb, IConfiguration configuration)
        {
            _logger = logger;
            _serviceDb = serviceDb;
            _configuration = configuration;
        }

        public async Task<StreetResponseDto> FetchAndProcessingStreetCreationAsync(StreetCreateRequestDto request)
        {
            var street = await _serviceDb.ReadStreetAsync(request.Name);
            if (street != null)
            {
                return new StreetResponseDto() { State = StreetOperationStates.Error, Message = STREET_ALREADY_EXISTING };
            }
            if (null == request.Points || request.Points.Count == 0)
            {
                return new StreetResponseDto() { State = StreetOperationStates.Error, Message = STREET_DOES_NOT_CONTAIN_POINTS };
            }
            var result = await _serviceDb.CreateStreetAsync(request.Name, Map(request.Points), request.Capacity);
            if (result)
            {
                return new StreetResponseDto() { State = StreetOperationStates.Success, Message = STREET_CREATED };
            }
            return new StreetResponseDto() { State = StreetOperationStates.Error };
        }

        public async Task<StreetResponseDto> FetchAndProcessingStreetDeletionsAsync(StreetDeleteRequestDto request)
        {
            var street = await _serviceDb.ReadStreetAsync(request.Name);
            if (null == street)
            {
                return new StreetResponseDto() { State = StreetOperationStates.Error, Message = STREET_NOT_EXISTING };
            }
            var result = await _serviceDb.DeleteStreetAsync(street);
            if (result)
            {
                return new StreetResponseDto() { State = StreetOperationStates.Success, Message = STREET_REMOVED };
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
                    var result = await AddNewPointAsync(street, newPoint);
                    if (result)
                    {
                        return new StreetResponseDto() { State = StreetOperationStates.Success, Message = NEW_POINT_ADDED };
                    }
                    else
                    {
                        return new StreetResponseDto() { State = StreetOperationStates.Error };
                    }
                }
                else
                {
                    return new StreetResponseDto() { State = StreetOperationStates.Error, Message = POINT_ALREADY_EXISTING };
                }
            }
            else
            {
                return new StreetResponseDto() { State = StreetOperationStates.Error, Message = STREET_NOT_EXISTING };
            }
        }

        private async Task<bool> AddNewPointAsync(StreetEntity street, Coordinate newPoint)
        {
            bool isSqlScript = _configuration.GetValue<bool>(USE_POSTGIS_FEATURE);
            if (isSqlScript)
            {
                return await _serviceDb.AddNewPointViaSqlScriptAsync(street.Id, newPoint);
            }
            else
            {
                var newPoints = StreetComputingUtility.CalculateNewPoints(street.Geometry, newPoint);
                return await _serviceDb.AddNewPointAsync(street.Id, newPoints.ToArray());
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
