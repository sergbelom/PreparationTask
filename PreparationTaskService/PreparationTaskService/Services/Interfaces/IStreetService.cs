using PreparationTaskService.DataTransfer.Streets;

namespace PreparationTaskService.Services.Interfaces
{
    public interface IStreetService
    {
        /// <summary>
        /// Fetch and processing the street creation
        /// </summary>
        /// <param name="streetCreateRequest"></param>
        /// <returns></returns>
        Task<StreetResponseHolderDto> FetchAndProcessingStreetCreationAsync(StreetCreateRequestDto streetCreateRequest);

        /// <summary>
        /// Fetch and processing the street deletion.
        /// </summary>
        /// <param name="streetDeleteRequest"></param>
        /// <returns></returns>
        Task<StreetResponseHolderDto> FetchAndProcessingStreetDeletionsAsync(StreetDeleteRequestDto streetDeleteRequest);

        /// <summary>
        /// Fetch and processing adding new point.
        /// </summary>
        /// <param name="streetAddPointRequest"></param>
        /// <returns></returns>
        Task<StreetResponseHolderDto> FetchAndProcessingAddingNewPointAsync(StreetAddPointRequestDto streetAddPointRequest);
    }
}
