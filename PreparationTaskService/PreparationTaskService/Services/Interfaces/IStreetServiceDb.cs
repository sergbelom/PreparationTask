using NetTopologySuite.Geometries;
using PreparationTaskService.DAL.Entities;

namespace PreparationTaskService.Services.Interfaces
{
    /// <summary>
    /// Service to access table STREETS
    /// </summary>
    public interface IStreetServiceDb
    {
        /// <summary>
        /// Create street
        /// </summary>
        /// <param name="streetId"></param>
        /// <param name="name"></param>
        /// <param name="points"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        Task<bool> CreateStreetAsync(string name, LineString lineString, int capacity);

        /// <summary>
        /// Read Streets by name
        /// </summary>
        /// <param name="streetName"></param>
        /// <returns></returns>
        Task<StreetEntity> ReadStreetAsync(string streetName);

        /// <summary>
        /// Delete street by StreetEntity
        /// </summary>
        /// <param name="streetEntity"></param>
        /// <returns></returns>
        Task<bool> DeleteStreetAsync(StreetEntity streetEntity);

        /// <summary>
        /// Add New Point (Thread safe inside)
        /// </summary>
        /// <param name="street"></param>
        /// <param name="newStreetPoint"></param>
        /// <returns></returns>
        Task<bool> AddNewPointAsync(int streetId, Coordinate[] newPoints);

        /// <summary>
        /// Add New Point via  Sql Script
        /// </summary>
        /// <param name="streetId"></param>
        /// <param name="newPoint"></param>
        /// <returns></returns>
        Task<bool> AddNewPointViaSqlScriptAsync(int streetId, Coordinate newPoint);
    }
}
