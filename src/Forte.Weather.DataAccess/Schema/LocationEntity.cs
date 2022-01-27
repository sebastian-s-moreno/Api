using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Forte.Weather.DataAccess.Schema
{
    public class LocationEntity
    {
        [Key]
        public string ID { get; set; } = "";
        public string Name { get; set; } = "";
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Timeserie { get; set; } = "";
    }
}
