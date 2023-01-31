using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace Business.Models
{
    public class CityModel:RecordBase
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public int CountryId { get; set; }
    }
}
