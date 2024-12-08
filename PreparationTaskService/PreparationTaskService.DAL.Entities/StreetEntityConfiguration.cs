using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PreparationTaskService.DAL.Entities
{
    public class StreetEntityConfiguration : IEntityTypeConfiguration<StreetEntity>
    {
        public void Configure(EntityTypeBuilder<StreetEntity> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
