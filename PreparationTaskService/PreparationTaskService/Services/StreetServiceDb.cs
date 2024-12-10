using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Npgsql;
using PreparationTaskService.DAL;
using PreparationTaskService.DAL.Entities;
using PreparationTaskService.Services.Interfaces;

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
                using (var dbContext = await _dbFactory.CreateDbContextAsync())
                {
                    dbContext.StreetsDbSet.Add(new StreetEntity()
                    {
                        Name = name,
                        Geometry = lineString,
                        Capacity = capacity
                    });
                    await dbContext.SaveChangesAsync();
                }
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Street not created. Internal Error.", ex);
            }
            return result;
        }

        public async Task<StreetEntity> ReadStreetAsync(string streetName)
        {
            StreetEntity resultStreet = null;
            try
            {
                using (var dbContext = await _dbFactory.CreateDbContextAsync())
                {
                    resultStreet = await dbContext.StreetsDbSet.FirstOrDefaultAsync(s => s.Name == streetName);
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
                using (var dbContext = await _dbFactory.CreateDbContextAsync())
                {
                    dbContext.StreetsDbSet.Remove(streetEntity);
                    await dbContext.SaveChangesAsync();
                }
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("StreetEntity not deleted. Internal Error.", ex);
            }
            return result;
        }

        public async Task<bool> AddNewPointAsync(int streetId, Coordinate[] newPoints)
        {
            bool result = false;
            using (var dbContext = await _dbFactory.CreateDbContextAsync())
            {
                var street = dbContext.StreetsDbSet.FirstOrDefaultAsync(s => s.Id == streetId).Result;
                if (street != null)
                {
                    street.Geometry = new LineString(newPoints);
                    await dbContext.SaveChangesAsync();
                    result = true;
                }
            }
            return result;
        }

        public async Task<bool> AddNewPointViaSqlScriptAsync(int streetId, Coordinate newPoint)
        {
            bool result = false;
            var sqlParameters = new[]
            {
                new NpgsqlParameter("@xcoord", newPoint.X),
                new NpgsqlParameter("@ycoord", newPoint.Y),
                new NpgsqlParameter("@streetId", streetId)
            };
            var sqlQuery = @"
                        UPDATE preptask.""STREETS""
                        SET ""Geometry"" = CASE
                            WHEN ST_Distance(ST_StartPoint(""Geometry""), ST_MakePoint(@xcoord, @ycoord)) <
                                 ST_Distance(ST_EndPoint(""Geometry""), ST_MakePoint(@xcoord, @ycoord))
                            THEN ST_AddPoint(""Geometry"", ST_MakePoint(@xcoord, @ycoord), 0)
                            ELSE ST_AddPoint(""Geometry"", ST_MakePoint(@xcoord, @ycoord))
                        END
                        WHERE ""Id"" = @streetId";

            try
            {
                using (var dbContext = await _dbFactory.CreateDbContextAsync())
                {
                    var executionSqlResult = await dbContext.Database.ExecuteSqlRawAsync(sqlQuery, sqlParameters);
                    if (executionSqlResult > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Execute Sql error", ex);
            }
            return result;
        }
    }
}
