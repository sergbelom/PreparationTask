using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using PreparationTaskService.DAL.Entities;

namespace PreparationTaskService.DAL
{
    public static class DatabaseContextExtensions
    {
        public static Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<StreetEntity> AddNewPointThreadSafe(this PrepTaskDatabaseContext dbContext, int streetId, Coordinate[] newPoints)
        {
            lock (dbContext) {

                var street = dbContext.StreetsDbSet.FirstOrDefaultAsync(s => s.Id == streetId).Result;
                if (street != null)
                {
                    street.Geometry = new LineString(newPoints);
                }
                return dbContext.StreetsDbSet.Update(street);
            }
        }
    }
}
