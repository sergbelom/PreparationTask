using Microsoft.EntityFrameworkCore;
using PreparationTaskService.DAL.Entities;

namespace PreparationTaskService.DAL
{
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Mapping for the STREETS table
        /// </summary>
        public DbSet<StreetEntity> StreetsDbSet { get; set; }
        public IQueryable<StreetEntity> Streets { get { return StreetsDbSet.AsQueryable(); } }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DALConstants.PREP_SCHEMA_NAME);
            modelBuilder.ApplyConfiguration(new StreetEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
