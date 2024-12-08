using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using PreparationTaskService.DAL;
using PreparationTaskService.DAL.Entities;
using PreparationTaskService.DataTransfer.Streets.Models;
using System.Collections.Generic;

namespace PreparationTaskService.Services
{
    public class StreetServiceDb : IStreetServiceDb
    {
        private readonly ILogger<StreetServiceDb> _logger;
        private readonly IDbContextFactory<PrepTaskDatabaseContext> _dbFactory;

        public StreetServiceDb(ILogger<StreetServiceDb> logger, IDbContextFactory<PrepTaskDatabaseContext> dbFactory)
        {
            _logger = logger;
            _dbFactory = dbFactory;
        }

        public async Task<bool> CreateStreetAsync(string name, LineString lineString, int capacity)
        {
            bool result = false;
            try
            {
                using (var database = await _dbFactory.CreateDbContextAsync())
                {
                    database.StreetsDbSet.Add(new StreetEntity()
                    {
                        Name = name,
                        Geometry = lineString,
                        Capacity = capacity
                    });
                    await database.SaveChangesAsync();
                }
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Street not created. Internal Error.", ex);
            }
            return result;
        }

        public async Task<List<StreetEntity>> ReadStreetsAsync(string streetName)
        {
            List<StreetEntity> resultStreets = null;
            try
            {
                using (var database = await _dbFactory.CreateDbContextAsync())
                {
                    resultStreets = await database.StreetsDbSet.Where(s => s.Name == streetName).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("No streets found. Internal Error.", ex);
            }
            return resultStreets;
        }

        public async Task<StreetEntity> ReadStreetAsync(string streetName)
        {
            StreetEntity resultStreet = null;
            try
            {
                using (var database = await _dbFactory.CreateDbContextAsync())
                {
                    resultStreet = await database.StreetsDbSet.FirstOrDefaultAsync(s => s.Name == streetName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("No street found. Internal Error.", ex);
            }
            return resultStreet;
        }

        public async Task<bool> DeleteStreetAsync(StreetEntity streetEntity)
        {
            bool result = false;
            try
            {
                using (var database = await _dbFactory.CreateDbContextAsync())
                {
                    database.StreetsDbSet.Remove(streetEntity);
                    await database.SaveChangesAsync();
                }
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("StreetEntity not deleted. Internal Error.", ex);
            }
            return result;
        }

        public async Task<bool> DeleteStreetsAsync(IEnumerable<StreetEntity> streetEntities)
        {
            bool result = false;
            try
            {
                using (var database = await _dbFactory.CreateDbContextAsync())
                {
                    foreach (var street in streetEntities)
                    {
                        database.StreetsDbSet.Remove(street);
                    }
                    await database.SaveChangesAsync();
                }
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("StreetEntities not deleted. Internal Error.", ex);
            }
            return result;
        }

        public async Task<bool> AddNewPointAsync(int streetId, Coordinate[] newPoints)
        {
            bool result = false;
            using (var database = await _dbFactory.CreateDbContextAsync())
            {
                var street = database.StreetsDbSet.FirstOrDefaultAsync(s => s.Id == streetId).Result;
                if (street != null)
                {
                    street.Geometry = new LineString(newPoints);
                    await database.SaveChangesAsync();
                    result = true;
                }
            }
            return result;
        }
    }
}
