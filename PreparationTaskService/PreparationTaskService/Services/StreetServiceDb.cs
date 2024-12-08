using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using PreparationTaskService.DAL;
using PreparationTaskService.DAL.Entities;
using PreparationTaskService.DataTransfer.Streets.Models;

namespace PreparationTaskService.Services
{
    public class StreetServiceDb : IStreetServiceDb
    {
        private readonly ILogger<StreetServiceDb> _logger;
        private readonly IDbContextFactory<DatabaseContext> _dbFactory;

        public StreetServiceDb(ILogger<StreetServiceDb> logger, IDbContextFactory<DatabaseContext> dbFactory)
        {
            _logger = logger;
            _dbFactory = dbFactory;
        }

        public async Task<bool> CreateStreetAsync(string name, LineString lineString, int capacity)
        {
            try
            {
                using var database = await _dbFactory.CreateDbContextAsync();
                database.StreetsDbSet.Add(new StreetEntity()
                {
                    Name = name,
                    Geometry = lineString,
                    Capacity = capacity
                });
                await database.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Street not created. Internal Error.", ex);
            }
            return false;
        }

        public async Task<List<StreetEntity>> ReadStreetsAsync(string streetName)
        {
            try
            {
                using var database = await _dbFactory.CreateDbContextAsync();
                var streets = await database.StreetsDbSet.Where(s => s.Name == streetName).ToListAsync();
                return streets;
            }
            catch (Exception ex)
            {
                _logger.LogError("No streets found. Internal Error.", ex);
            }
            return new List<StreetEntity>();
        }

        public async Task<StreetEntity> ReadStreetAsync(string streetName)
        {
            try
            {
                using var database = await _dbFactory.CreateDbContextAsync();
                var street = await database.StreetsDbSet.FirstOrDefaultAsync(s => s.Name == streetName);
                if (street != null)
                {
                    return street;
                }
                return new StreetEntity();
            }
            catch (Exception ex)
            {
                _logger.LogError("No street found. Internal Error.", ex);
            }
            return new StreetEntity();
        }

        public async Task<bool> DeleteStreetAsync(StreetEntity streetEntity)
        {
            try
            {
                using var database = await _dbFactory.CreateDbContextAsync();
                database.StreetsDbSet.Remove(streetEntity);
                await database.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("StreetEntity not deleted. Internal Error.", ex);
            }
            return false;
        }

        public async Task<bool> DeleteStreetsAsync(IEnumerable<StreetEntity> streetEntities)
        {
            try
            {
                using var database = await _dbFactory.CreateDbContextAsync();
                foreach(var street in streetEntities)
                {
                    database.StreetsDbSet.Remove(street);
                }
                await database.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("StreetEntities not deleted. Internal Error.", ex);
            }
            return false;
        }

        public async Task<bool> AddNewPointAsync(int streetId, Coordinate[] newPoints)
        {
            using var database = await _dbFactory.CreateDbContextAsync();
            var street = database.StreetsDbSet.FirstOrDefaultAsync(s => s.Id == streetId).Result;
            if (street != null)
            {
                street.Geometry = new LineString(newPoints);
                await database.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
