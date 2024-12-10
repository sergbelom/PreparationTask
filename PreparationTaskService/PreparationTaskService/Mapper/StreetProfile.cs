using AutoMapper;
using NetTopologySuite.Geometries;
using PreparationTaskService.DataTransfer.Streets;
using PreparationTaskService.DataTransfer.Streets.Models;

namespace PreparationTaskService.Mapper
{
    public class StreetProfile : Profile
    {
        public StreetProfile() {

            CreateMap<StreetResponseDto, StreetResponseHolderDto>()
                .ForPath(dst => dst.StreetResponse, opt => opt.MapFrom(src => src));

            CreateMap<StreetOperationStates, StreetResponseHolderDto>()
                .ForMember(dest => dest.StreetResponse, opt => opt.MapFrom(src => new StreetResponseDto
                {
                    State = src,
                    Message = string.Empty,
                }));

            CreateMap<(StreetOperationStates State, string Message), StreetResponseHolderDto>()
                .ForMember(dest => dest.StreetResponse, opt => opt.MapFrom(src => new StreetResponseDto
                {
                    State = src.State,
                    Message = src.Message
                }));

            CreateMap<StreetPoint, Coordinate>()
                .ConstructUsing(sp => new Coordinate(sp.Longitude, sp.Latitude));

            CreateMap<IEnumerable<StreetPoint>, LineString>()
                .ConstructUsing(streetPoints => new LineString(streetPoints.Select(sp => new Coordinate(sp.Longitude, sp.Latitude)).ToArray()));

        }
    }
}
