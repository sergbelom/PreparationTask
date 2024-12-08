using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreparationTaskService.DAL.Entities;

[Table("STREETS")]
public class StreetEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    public LineString Geometry { get; set; }

    [Required]
    public int Capacity { get; set; }
}
