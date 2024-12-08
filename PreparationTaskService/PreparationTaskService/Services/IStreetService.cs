using PreparationTaskService.DataTransfer.Streets;

namespace PreparationTaskService.Services
{
    public interface IStreetService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="streetCreateRequest"></param>
        /// <returns></returns>
        Task<StreetResponseDto> FetchAndProcessingStreetCreationAsync(StreetCreateRequestDto streetCreateRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streetDeleteRequest"></param>
        /// <returns></returns>
        Task<StreetResponseDto> FetchAndProcessingStreetDeletionsAsync(StreetDeleteRequestDto streetDeleteRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streetAddPointRequest"></param>
        /// <returns></returns>
        Task<StreetResponseDto> FetchAndProcessingAddingNewPointAsync(StreetAddPointRequestDto streetAddPointRequest);
    }
}
