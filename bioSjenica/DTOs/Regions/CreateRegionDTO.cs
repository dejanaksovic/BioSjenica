using bioSjenica.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace bioSjenica.DTOs.Regions
{
    public class CreateRegionDTO
    {
        public string Name { get; set; }
        public float Area { get; set; }
        public string Villages { get; set; }
        public string ProtectionType { get; set; }
        public List<string>? AnimalsCommonOrLatinicNames { get; set; }
        public List<string>? PlantsCommonOrLatinicNames { get; set; }
        public List<int>? GroundNumbers { get; set; }
    }

}
