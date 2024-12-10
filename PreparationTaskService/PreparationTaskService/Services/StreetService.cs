using AutoMapper;
using NetTopologySuite.Geometries;
using PreparationTaskService.DAL.Entities;
using PreparationTaskService.DataTransfer.Streets;
using PreparationTaskService.DataTransfer.Streets.Models;
using PreparationTaskService.Services.Interfaces;
using static PreparationTaskService.Common.StaticMessages;
using static PreparationTaskService.Common.Statics;

namespace PreparationTaskService.Services
{
    public class StreetService : IStreetService
    {
        private readonly ILogger<StreetService> _logger;
        private readonly IStreetServiceDb _serviceDb;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public StreetService(ILogger<StreetService> logger, IStreetServiceDb serviceDb, IConfiguration configuration, IMapper mapper)
        {
            _logger = logger;
            _serviceDb = serviceDb;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<StreetResponseHolderDto> FetchAndProcessingStreetCreationAsync(StreetCreateRequestDto request)
        {
            if (null == request.Points || request.Points.Count == 0)
            {
                return _mapper.Map<StreetResponseHolderDto>((StreetOperationStates.Error, STREET_DOES_NOT_CONTAIN_POINTS));
            }
            var street = await _serviceDb.ReadStreetAsync(request.Name);
            if (street != null)
            {
                return _mapper.Map<StreetResponseHolderDto>((StreetOperationStates.Error, STREET_ALREADY_EXISTING));
            }
            var result = await _serviceDb.CreateStreetAsync(request.Name, _mapper.Map<LineString>(request.Points), request.Capacity);
            if (result)
            {
                return _mapper.Map<StreetResponseHolderDto>((StreetOperationStates.Success, STREET_CREATED));
            }
            return _mapper.Map<StreetResponseHolderDto>((StreetOperationStates.Error));
        }

        public async Task<StreetResponseHolderDto> FetchAndProcessingStreetDeletionsAsync(StreetDeleteRequestDto request)
        {
            var street = await _serviceDb.ReadStreetAsync(request.Name);
            if (null == street)
            {
                return _mapper.Map<StreetResponseHolderDto>((StreetOperationStates.Error, STREET_NOT_EXISTING));
            }
            var result = await _serviceDb.DeleteStreetAsync(street);
            if (result)
            {
                return _mapper.Map<StreetResponseHolderDto>((StreetOperationStates.Success, STREET_REMOVED));
            }
            return _mapper.Map<StreetResponseHolderDto>((StreetOperationStates.Error));
        }

        public async Task<StreetResponseHolderDto> FetchAndProcessingAddingNewPointAsync(StreetAddPointRequestDto streetAddPointRequest)
        {
            var street = await _serviceDb.ReadStreetAsync(streetAddPointRequest.Name);
            if (street != null)
            {
                var newPoint = _mapper.Map<Coordinate>(streetAddPointRequest.NewPoint);
                if (!street.Geometry.IsCoordinate(newPoint))
                {
                    var result = await AddNewPointAsync(street, newPoint);
                    if (result)
                    {
                        return _mapper.Map<StreetResponseHolderDto>((StreetOperationStates.Success, NEW_POINT_ADDED));
                    }
                    else
                    {
                        return _mapper.Map<StreetResponseHolderDto>((StreetOperationStates.Error));
                    }
                }
                else
                {
                    return _mapper.Map<StreetResponseHolderDto>((StreetOperationStates.Error, POINT_ALREADY_EXISTING));
                }
            }
            else
            {
                return _mapper.Map<StreetResponseHolderDto>((StreetOperationStates.Error, STREET_NOT_EXISTING));
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
    }
}
