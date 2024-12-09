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
        Task<StreetResponseHolderDto> FetchAndProcessingStreetCreationAsync(StreetCreateRequestDto streetCreateRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streetDeleteRequest"></param>
        /// <returns></returns>
        Task<StreetResponseHolderDto> FetchAndProcessingStreetDeletionsAsync(StreetDeleteRequestDto streetDeleteRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streetAddPointRequest"></param>
        /// <returns></returns>
        Task<StreetResponseHolderDto> FetchAndProcessingAddingNewPointAsync(StreetAddPointRequestDto streetAddPointRequest);
    }
}
