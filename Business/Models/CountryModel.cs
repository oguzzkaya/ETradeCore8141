using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class CountryModel:RecordBase
    {
        [Required]
        [StringLength(100)]
        //[Column("Adi", TypeName = "varchar(100)")]
        public string Name { get; set; }
    }
}
